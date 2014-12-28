using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Commands;
using ReeperCommon.GameLoadState.Handlers;

namespace ReeperCommon.GameLoadState.Factories
{
    public class ConstructCommandFactory : IConstructCommandFactory
    {
        public IConstructCommand Create(ILoadStateHandler handler, LoadStateMarker.State state)
        {
            return new ConstructCommand(handler, state);
        }
    }
}
