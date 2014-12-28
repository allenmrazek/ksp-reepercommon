using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.GameLoadState
{
    class LoadState_Immediate : GameLoadStateObserver
    {
        private void Start()
        {
            Receiver.CreateTypesFor(Attributes.LoadStateMarker.State.Immediate);
        }
    }
}
