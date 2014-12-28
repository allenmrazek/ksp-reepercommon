using ReeperCommon.GameLoadState.Attributes;

namespace ReeperCommon.GameLoadState.Triggers
{
    public delegate void TriggerCallback(ILoadStateTrigger trigger, LoadStateMarker.State state);

    public interface ILoadStateTrigger
    {
        void UpdateState();
    }
}