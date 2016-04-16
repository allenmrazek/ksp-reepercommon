using System;

namespace ReeperCommon.AssetBundleLoading
{
    public class AssetBundleNotFoundException : Exception
    {
        public AssetBundleNotFoundException(string fullPath, string relativePath)
            : base(string.Format("AssetBundle at '{0}' [relative path '{1}'] not found", fullPath, relativePath))
        {
        }
    }
}