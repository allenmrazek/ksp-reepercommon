using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Commands;
using ReeperCommon.GameLoadState.Handlers;

namespace ReeperCommon.GameLoadState.Factories
{
    public interface IConstructCommandFactory
    {
        IConstructCommand Create(ILoadStateHandler handler, LoadStateMarker.State state);
    }
}
