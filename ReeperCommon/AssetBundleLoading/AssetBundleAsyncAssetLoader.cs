using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReeperCommon.AssetBundleLoading
{
    public class AssetBundleAsyncAssetLoader : AssetBundleAssetLoader
    {
        private readonly MonoBehaviour _host;

        public AssetBundleAsyncAssetLoader([NotNull] MonoBehaviour host)
        {
            if (host == null) throw new ArgumentNullException("host");
            _host = host;
        }


        // ReSharper disable once UnusedMember.Global
        public IEnumerator InjectAssetsAsync([NotNull] object fieldOwner)
        {
            if (fieldOwner == null) throw new ArgumentNullException("fieldOwner");

            var targetFields = GetFieldsOf(fieldOwner).Where(fi => GetAttribute(fi).Any()).ToList();

            var pathsOfBundles =
                targetFields
                .Select(fi => GetAssetBundleFullPath(fi.DeclaringType, GetAttribute(fi).Value))
                    .OrderByDescending(f => f)
                    .Distinct();

            // make sure all of the needed AssetBundles are loaded
            foreach (var bundlePath in pathsOfBundles)
            {
                if (!GetLoadedAssetBundle(bundlePath).Any())
                    yield return _host.StartCoroutine(LoadAssetBundleAsync(bundlePath));
            }

            var fieldValues = new Dictionary<FieldInfo, Object>();

            // load values that will be injected
            foreach (var field in targetFields)
            {
                var attr = GetAttribute(field).Value;
                var assetBundlePath = GetAssetBundleFullPath(field.DeclaringType, attr);
                var bundle = GetLoadedAssetBundle(assetBundlePath);

                if (!bundle.Any())
                    throw new InvalidOperationException("Somehow the expected AssetBundle is not loaded for " +
                                                        field.Name + ":" + field.FieldType + " on " +
                                                        field.DeclaringType + ", " + attr);

                yield return _host.StartCoroutine(LoadFieldValueAsync(bundle.Value, field, attr, fieldValues));
            }

            AssignFields(fieldOwner, fieldValues);
        }


        protected static IEnumerator LoadAssetBundleAsync(string assetBundlePath)
        {
            if (string.IsNullOrEmpty(assetBundlePath))
                throw new ArgumentException("cannot be null or empty", "assetBundlePath");

            if (!File.Exists(assetBundlePath)) throw new FileNotFoundException("File not found!", assetBundlePath);

            if (LoadedBundles.Keys.Any(k => k == assetBundlePath))
                throw new ArgumentException("AssetBundle '" + assetBundlePath + "' has already been loaded");

            var wwwLoad = new WWW(Uri.EscapeUriString(Application.platform == RuntimePlatform.WindowsPlayer ? "file:///" + assetBundlePath : "file://" + assetBundlePath));

            yield return wwwLoad;

            if (!string.IsNullOrEmpty(wwwLoad.error))
                throw new AsyncAssetBundleLoadException(wwwLoad.error);

            var bundle = wwwLoad.assetBundle;

            if (bundle == null)
                throw new ArgumentException("Failed to create AssetBundle from '" + assetBundlePath + "'");

            LoadedBundles.Add(assetBundlePath, bundle);
        }


        protected static IEnumerator LoadFieldValueAsync(
            AssetBundle bundle, 
            FieldInfo field, 
            AssetBundleAssetAttribute asset,
            Dictionary<FieldInfo, UnityEngine.Object> fieldValues)
        {
            if (!bundle.GetAllAssetNames().Any(assetName => string.Equals(assetName, asset.Name)))
                throw new AssetNotFoundException(asset);

            var assetRequest = bundle.LoadAssetAsync(asset.Name, GetAssetTypeToLoad(field.FieldType));

            yield return assetRequest;

            var loadedAsset = assetRequest.asset;

            if (loadedAsset == null)
                throw new FailedToLoadAssetException(asset, field.FieldType);

            fieldValues.Add(field, ApplyCreationOptions(ConvertLoadedAssetToCorrectType(loadedAsset, field.FieldType, asset), field.FieldType, asset));
        }
    }
}
