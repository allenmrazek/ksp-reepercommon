using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReeperCommon.AssetBundleLoading
{
    // ReSharper disable once UnusedMember.Global
    public class AssetBundleAssetLoader : IEnumerable<AssetBundle>
    {
        protected static readonly Dictionary<string, AssetBundle> LoadedBundles =
            new Dictionary<string, AssetBundle>();

        public static ReadOnlyCollection<KeyValuePair<string, AssetBundle>> Bundles
        {
            get { return new ReadOnlyCollection<KeyValuePair<string, AssetBundle>>(LoadedBundles.Select(kvp => kvp).ToList()); }
        }  

        // ReSharper disable once UnusedMember.Global
        public void InjectAssets([NotNull] object target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var fieldValues = new Dictionary<FieldInfo, Object>();

            foreach (var f in GetFieldsOf(target))
            {
                var assetAttribute = GetAttribute(f);

                if (!assetAttribute.Any()) continue;

                var assetBundlePath = GetAssetBundleFullPath(target.GetType(), assetAttribute.Value);

                // cache all the field values we're going to set. If something goes wrong,
                // we don't want to have the target in a half-injected mutated state
                fieldValues.Add(f,
                    LoadAssetImmediate(GetLoadedAssetBundle(assetBundlePath).Or(LoadAssetBundle(assetBundlePath)),
                        f.FieldType, assetAttribute.Value));
            }

            AssignFields(target, fieldValues);
        }





        /// <summary>
        /// Loads an asset synchronously
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="assetType"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected static Object LoadAssetImmediate(AssetBundle bundle, Type assetType, AssetBundleAssetAttribute asset)
        {
            if (!bundle.GetAllAssetNames().Any(assetName => string.Equals(assetName, asset.Name)))
                throw new AssetNotFoundException(asset);

            var loadedAsset = bundle.LoadAsset(asset.Name, GetAssetTypeToLoad(assetType));

            if (loadedAsset == null)
                throw new FailedToLoadAssetException(asset, assetType);

            return ApplyCreationOptions(ConvertLoadedAssetToCorrectType(loadedAsset, assetType, asset), assetType, asset);
        }


        /// <summary>
        /// If the asset was loaded as a GameObject and the field type is some kind of component, we must grab the actual component
        /// out of the GameObject as that's the value that's really going to be injected
        /// </summary>
        /// <param name="loadedAsset"></param>
        /// <param name="assetFieldType"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected static Object ConvertLoadedAssetToCorrectType(Object loadedAsset, Type assetFieldType, AssetBundleAssetAttribute asset)
        {
            if (!AssetShouldBeLoadedAsGameObject(assetFieldType)) return loadedAsset;

            var goAsset = loadedAsset as GameObject;
            if (goAsset == null)
                throw new FailedToLoadAssetException(asset, typeof (GameObject));

            return assetFieldType == typeof (GameObject) ? (Object) goAsset : goAsset.GetComponent(assetFieldType)
                .IfNull(() =>
                {
                    throw new FailedToLoadAssetException (asset, assetFieldType);
                });
        }


        /// <summary>
        /// Taking a loaded asset, this will create an instance or whatever creation options are desired
        /// </summary>
        /// <param name="loadedAsset"></param>
        /// <param name="assetFieldType"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected static Object ApplyCreationOptions(Object loadedAsset, Type assetFieldType, AssetBundleAssetAttribute asset)
        {
            switch (asset.CreationStyle)
            {
                case AssetBundleAssetAttribute.AssetCreationStyle.Prefab:
                    return loadedAsset;

                case AssetBundleAssetAttribute.AssetCreationStyle.Instance:
                    {
                        if (!typeof(Component).IsAssignableFrom(assetFieldType)) return Object.Instantiate(loadedAsset);

                        var c = (Component)loadedAsset;

                        return Object.Instantiate(loadedAsset, c.transform.position, c.transform.rotation);
                    }

                default:
                    throw new NotImplementedException(asset.CreationStyle.ToString());
            }
        }


        protected static Maybe<AssetBundle> GetLoadedAssetBundle(string assetBundlePath)
        {
            if (string.IsNullOrEmpty(assetBundlePath))
                throw new ArgumentException("cannot be null or empty", "assetBundlePath");

            if (!File.Exists(assetBundlePath)) throw new FileNotFoundException("File not found!", assetBundlePath);

            AssetBundle bundle;

            return LoadedBundles.TryGetValue(assetBundlePath, out bundle) ? bundle.ToMaybe() : Maybe<AssetBundle>.None;
        }


        protected static AssetBundle LoadAssetBundle(string assetBundlePath)
        {
            if (string.IsNullOrEmpty(assetBundlePath))
                throw new ArgumentException("cannot be null or empty", "assetBundlePath");

            if (!File.Exists(assetBundlePath)) throw new FileNotFoundException("File not found!", assetBundlePath);

            if (LoadedBundles.Keys.Any(k => k == assetBundlePath))
                throw new ArgumentException("AssetBundle '" + assetBundlePath + "' has already been loaded");

            var bundle = AssetBundle.CreateFromMemoryImmediate(File.ReadAllBytes(assetBundlePath));

            if (bundle == null)
                throw new ArgumentException("Failed to create AssetBundle from '" + assetBundlePath + "'");

            LoadedBundles.Add(assetBundlePath, bundle);

            return bundle;
        }


        protected static void AssignFields(object fieldOwner, Dictionary<FieldInfo, Object> fieldValues)
        {
            foreach (var kvp in fieldValues)
                kvp.Key.SetValue(kvp.Key.IsStatic ? null : fieldOwner, kvp.Value);
        }


        // ReSharper disable once UnusedMember.Global
        public void UnloadAllBundles(bool unloadAllLoadedObjects = false)
        {
            foreach (var bundle in LoadedBundles.Values)
            {
                try
                {
                    bundle.Unload(unloadAllLoadedObjects);
                }
                catch (Exception e)
                {
                    Log.Warning("Exception while unloading an AssetBundle: " + e);
                }
            }

            LoadedBundles.Clear();
        }

 
        protected static string GetAssetBundleFullPath(
            [NotNull] Type fieldOwnerType,
            [NotNull] AssetBundleAssetAttribute attribute)
        {
            if (fieldOwnerType == null) throw new ArgumentNullException("fieldOwnerType");
            if (attribute == null) throw new ArgumentNullException("attribute");
            var assemblyIdentifier = GetGameDatabaseUrlOfAssembly(fieldOwnerType.Assembly);

            // combine relative url with ownerType assembly url to get url of AssetBundle (including its name and extension),
            // then combine it with GameData path to come up with a fully qualified path
            var bundleFullPath =
                Path.GetFullPath(
                    Path.Combine(
                        Path.Combine(KSPUtil.ApplicationRootPath, "GameData" + assemblyIdentifier.Url),
                        attribute.AssetBundleRelativeUrl).Replace('\\', Path.DirectorySeparatorChar)
                        .Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(bundleFullPath))
                throw new AssetBundleNotFoundException(bundleFullPath, attribute.AssetBundleRelativeUrl);

            return bundleFullPath;
        }


        private static IUrlIdentifier GetGameDatabaseUrlOfAssembly(Assembly assembly)
        {
            var loadedAssembly = AssemblyLoader.loadedAssemblies.FirstOrDefault(la => ReferenceEquals(la.assembly, assembly)).ToMaybe();

            if (!loadedAssembly.Any())
                throw new LoadedAssemblyNotFoundException(assembly);

            if (String.IsNullOrEmpty(loadedAssembly.Value.url))
                throw new LoadedAssemblyNotFoundException(loadedAssembly.Value);

            return new KSPUrlIdentifier(loadedAssembly.Value.url, UrlType.Assembly);
        }


        protected static Maybe<AssetBundleAssetAttribute> GetAttribute(FieldInfo fi)
        {
            return fi.GetCustomAttributes(false).OfType<AssetBundleAssetAttribute>().SingleOrDefault().ToMaybe();
        }

        protected static IEnumerable<FieldInfo> GetFieldsOf(object target)
        {
            return target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static);
        }

        public IEnumerator<AssetBundle> GetEnumerator()
        {
            return LoadedBundles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// According to the Unity docs, as of Unity5+ Component-type prefabs should be loaded as GameObjects
        /// </summary>
        /// <param name="assetType"></param>
        /// <returns></returns>
        protected static bool AssetShouldBeLoadedAsGameObject(Type assetType)
        {
            return typeof(Component).IsAssignableFrom(assetType);
        }


        /// <summary>
        /// According to the Unity docs, as of Unity5+ Component-type prefabs should be loaded as GameObjects
        /// </summary>
        /// <param name="assetType"></param>
        /// <returns></returns>
        protected static Type GetAssetTypeToLoad(Type assetType)
        {
            return AssetShouldBeLoadedAsGameObject(assetType) ? typeof(GameObject) : assetType;
        }
    }
}