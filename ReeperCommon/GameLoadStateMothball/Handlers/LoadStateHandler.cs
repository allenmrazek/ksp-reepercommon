using System;
using System.Collections.Generic;
using ReeperCommon.Extensions;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Factories;
using ReeperCommon.GameLoadState.Providers;
using ReeperCommon.GameLoadState.Triggers;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Handlers
{
    


    public class LoadStateHandler : ILoadStateHandler
    {
        private readonly ILoadStateMarkedTypeProvider _markedTypeProvider;


        public LoadStateHandler(ITriggerFactory triggerFactory, ILoadStateMarkedTypeProvider typeProvider)
        {
            if (triggerFactory.IsNull())
                throw new ArgumentNullException("triggerFactory");

            if (typeProvider.IsNull())
                throw new ArgumentNullException("typeProvider");

            _markedTypeProvider = typeProvider;

            foreach (LoadStateMarker.State state in Enum.GetValues(typeof (LoadStateMarker.State)))
                triggerFactory.Create(this, state);
        }



        // create any marked types for specified load state
        public void CreateMarkedTypesFor(LoadStateMarker.State state)
        {
            foreach (var t in _markedTypeProvider.GetMarkedTypesFor(state))
            {
                var go = new GameObject("LoadState." + state + "." + t.Name);
                go.AddComponent(t);
            }
        }
    }
}
