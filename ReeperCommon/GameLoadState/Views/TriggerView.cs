using System.Collections.Generic;
using ReeperCommon.Extensions;
using ReeperCommon.GameLoadState.Triggers;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Views
{
    internal class TriggerView : MonoBehaviour, ITriggerView
    {
        private List<ILoadStateTrigger> _triggers;

        private void Awake()
        {
            _triggers = new List<ILoadStateTrigger>();
        }


        private void Update()
        {
            _triggers.ForEach(t => t.UpdateState());
        }



        public void AddTrigger(ILoadStateTrigger trigger)
        {
            if (!_triggers.Contains(trigger))
                _triggers.Add(trigger);
        }

        public void RemoveTrigger(ILoadStateTrigger trigger)
        {
            if (_triggers.Contains(trigger))
                _triggers.Remove(trigger);
        }
    }
}
