using ReeperCommon.GameLoadState.Triggers;

namespace ReeperCommon.GameLoadState.Views
{
    public interface ITriggerView
    {
        void AddTrigger(ILoadStateTrigger trigger);
        void RemoveTrigger(ILoadStateTrigger trigger);
    }
}
