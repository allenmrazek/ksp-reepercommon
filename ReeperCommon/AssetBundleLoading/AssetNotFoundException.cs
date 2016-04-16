using System;

namespace ReeperCommon.AssetBundleLoading
{
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException(AssetBundleAssetAttribute attr)
            : base("Asset not found: " + attr)
        {
        }
    }
}