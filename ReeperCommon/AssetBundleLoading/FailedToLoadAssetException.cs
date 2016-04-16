using System;

namespace ReeperCommon.AssetBundleLoading
{
    public class FailedToLoadAssetException : Exception
    {
        public FailedToLoadAssetException(AssetBundleAssetAttribute attribute, Type assetType)
            : base(string.Format("Failed to load asset of type {0} with attribute {1}", assetType.Name, attribute))
        {

        }
    }
}