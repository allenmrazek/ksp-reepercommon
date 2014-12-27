using System;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Handlers;
using ReeperCommon.GameLoadState.Triggers;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Factories
{
    public class TriggerFactory
    {
        public ILoadStateTrigger Create(ILoadStateHandler handler, LoadStateMarker.State forState)
        {
            var go = new GameObject(string.Format("Trigger:{0}", forState));

            var trigger = go.AddComponent(ResolveType(forState)) as ILoadStateTrigger;
            trigger.SetCallback(handler.Notify);

            return trigger;
        }



        private Type ResolveType(LoadStateMarker.State state)
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
                    return typeof (TriggerImmediate);

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
