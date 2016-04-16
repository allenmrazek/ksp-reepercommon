using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.AssetBundleLoading
{
    class AsyncAssetBundleLoadException : Exception
    {
        public AsyncAssetBundleLoadException(string message) : base(message)
        {
            
        }
    }
}
