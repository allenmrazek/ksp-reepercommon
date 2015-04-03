namespace ReeperCommon.GameLoadState
{
    class LoadState_Immediate : GameLoadStateObserver
    {
        private void Start()
        {
            Receiver.CreateTypesFor(Attributes.LoadStateMarker.State.Immediate);
        }
    }
}
