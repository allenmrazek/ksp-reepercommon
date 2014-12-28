using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Handlers;
using ReeperCommon.GameLoadState.Triggers;

namespace ReeperCommon.GameLoadState.Factories
{
    public interface ITriggerFactory
    {
        ILoadStateTrigger Create(ILoadStateHandler handler, LoadStateMarker.State forState);
    }
}
