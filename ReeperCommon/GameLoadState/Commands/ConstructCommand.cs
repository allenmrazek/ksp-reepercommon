using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Handlers;
using ReeperCommon.GameLoadState.Views;

namespace ReeperCommon.GameLoadState.Commands
{
    class ConstructCommand : IConstructCommand
    {
        private readonly ILoadStateHandler _handler;
        private readonly LoadStateMarker.State _state;

        public ConstructCommand(ILoadStateHandler handler, LoadStateMarker.State state)
        {
            if (handler.IsNull())
                throw new ArgumentNullException("handler");

            if (state.IsNull())
                throw new ArgumentNullException("state");

            _handler = handler;
            _state = state;
        }

        public void Execute()
        {
            _handler.CreateMarkedTypesFor(_state);
        }
    }
}
