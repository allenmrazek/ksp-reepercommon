using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;

namespace ReeperCommon.GameLoadState
{
    class LoadState_FileTreeReady : GameLoadStateObserver
    {
        IEnumerator Start()
        {
            while (GameDatabase.Instance.IsNull() || GameDatabase.Instance.root.IsNull())
                yield return 0;

            Receiver.CreateTypesFor(Attributes.LoadStateMarker.State.FileTreeReady);
        }
    }
}
