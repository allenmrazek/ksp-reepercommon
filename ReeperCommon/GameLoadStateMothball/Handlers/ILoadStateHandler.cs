using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Triggers;

namespace ReeperCommon.GameLoadState.Handlers
{
    public interface ILoadStateHandler
    {
        void CreateMarkedTypesFor(LoadStateMarker.State state);
    }
}
