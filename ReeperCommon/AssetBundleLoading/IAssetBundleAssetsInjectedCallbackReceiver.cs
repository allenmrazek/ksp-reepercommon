namespace ReeperCommon.AssetBundleLoading
{
    public interface IAssetBundleAssetsInjectedCallbackReceiver
    {
        void BeforeAssetInjection();
        void AfterAssetInjection();
    }
}
