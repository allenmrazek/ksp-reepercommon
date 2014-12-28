using System;
using ReeperCommon.Extensions;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Commands;
using ReeperCommon.GameLoadState.Handlers;
using ReeperCommon.GameLoadState.Triggers;
using ReeperCommon.GameLoadState.Views;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Factories
{
    public class TriggerFactory : ITriggerFactory
    {
        private readonly IConstructCommandFactory _commandFactory;
        private readonly ITriggerView _triggerView;

        public TriggerFactory(ITriggerView view, IConstructCommandFactory commandFactory)
        {
            if (view.IsNull())
                throw new ArgumentNullException("view");

            if (commandFactory.IsNull())
                throw new ArgumentNullException("commandFactory");

            _commandFactory = commandFactory;
            _triggerView = view;
        }



        public ILoadStateTrigger Create(ILoadStateHandler handler, LoadStateMarker.State forState)
        {
            return ConstructTriggerFor(handler, forState);
        }



        private ILoadStateTrigger ConstructTriggerFor(ILoadStateHandler handler, LoadStateMarker.State state)
        {
            switch (state)
            {
                case LoadStateMarker.State.AfterModelsLoaded:
                    throw new NotImplementedException();

                case LoadStateMarker.State.AfterTexturesLoaded:
                    throw new NotImplementedException();

                case LoadStateMarker.State.FileTreeReady:
                    throw new NotImplementedException();

                case LoadStateMarker.State.Flight:
                    throw new NotImplementedException();

                case LoadStateMarker.State.Immediate:
                    return new TriggerImmediate(_triggerView, _commandFactory.Create(handler, state));

                case LoadStateMarker.State.MainMenu:
                    throw new NotImplementedException();

                case LoadStateMarker.State.SpaceCenter:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
