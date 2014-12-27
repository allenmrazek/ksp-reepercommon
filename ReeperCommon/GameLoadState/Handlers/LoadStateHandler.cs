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
        private readonly List<ILoadStateTrigger> _triggers = new List<ILoadStateTrigger>();


        public LoadStateHandler(TriggerFactory triggerFactory, ILoadStateMarkedTypeProvider typeProvider)
        {
            if (triggerFactory.IsNull())
                throw new ArgumentNullException("triggerFactory");

            if (typeProvider.IsNull())
                throw new ArgumentNullException("typeProvider");

            _markedTypeProvider = typeProvider;

            foreach (LoadStateMarker.State state in Enum.GetValues(typeof (LoadStateMarker.State)))
                AddTrigger(triggerFactory.Create(this, state));
        }

        ~LoadStateHandler()
        {
            _triggers.ForEach(t => t.SetCallback(null));
        }


        // create any marked types for specified load state
        public void Notify(ILoadStateTrigger trigger, LoadStateMarker.State state)
        {
            RemoveTrigger(trigger);

            foreach (var t in _markedTypeProvider.GetMarkedTypesFor(state))
            {
                var go = new GameObject(string.Format("LoadStateMarker.{0}.{1}", trigger, t.FullName));
                go.AddComponent(t);
            }
        }



        private void AddTrigger(ILoadStateTrigger trigger)
        {
            if (!_triggers.Contains(trigger)) _triggers.Add(trigger);
        }



        private void RemoveTrigger(ILoadStateTrigger trigger)
        {
            if (_triggers.Contains(trigger)) _triggers.Remove(trigger);
        }
    }
}
