using System;

namespace ReeperCommon.AssetBundleLoading
{
    class AsyncAssetBundleLoadException : Exception
    {
        public AsyncAssetBundleLoadException(string message) : base(message)
        {
            
        }
    }
}
