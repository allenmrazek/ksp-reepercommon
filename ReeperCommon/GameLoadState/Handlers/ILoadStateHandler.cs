using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Triggers;

namespace ReeperCommon.GameLoadState.Handlers
{
    public interface ILoadStateHandler
    {
        void Notify(ILoadStateTrigger trigger, LoadStateMarker.State state);
    }
}
