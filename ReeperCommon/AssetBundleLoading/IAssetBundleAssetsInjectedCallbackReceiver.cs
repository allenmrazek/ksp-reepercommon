namespace ReeperCommon.AssetBundleLoading
{
    public interface IAssetBundleAssetsInjectedCallbackReceiver
    {
        void BeforeAssetInjection(IAssetBundleAssetLoader assetLoader);
        void AfterAssetInjection(IAssetBundleAssetLoader assetLoader);
    }
}
