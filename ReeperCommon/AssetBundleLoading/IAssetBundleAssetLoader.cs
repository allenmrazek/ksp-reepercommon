using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace ReeperCommon.AssetBundleLoading
{
    public interface IAssetBundleAssetLoader
    {
        void InjectAssets([NotNull] object target);
        IEnumerator InjectAssetsAsync([NotNull] object fieldOwner);

        ReadOnlyCollection<KeyValuePair<string, AssetBundle>> Bundles { get; }
    }
}
