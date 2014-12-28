using System;
using ReeperCommon.Extensions;
using ReeperCommon.GameLoadState.Attributes;
using ReeperCommon.GameLoadState.Commands;
using ReeperCommon.GameLoadState.Views;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Triggers
{
    internal class TriggerImmediate :  ILoadStateTrigger
    {
        private readonly IConstructCommand _command;
        private readonly ITriggerView _view;
        
        public TriggerImmediate(ITriggerView view, IConstructCommand command)
        {
            if (view.IsNull())
                throw new ArgumentNullException("view");

            if (command.IsNull())
                throw new ArgumentNullException("command");

            _view = view;
            _command = command;
        }


        public void UpdateState()
        {
            // logic goes here
            // and then at some point:
            _command.Execute();
            _view.RemoveTrigger(this);
        }
    }
}
