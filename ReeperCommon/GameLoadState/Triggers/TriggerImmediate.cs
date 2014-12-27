using ReeperCommon.GameLoadState.Attributes;
using UnityEngine;

namespace ReeperCommon.GameLoadState.Triggers
{
    internal class TriggerImmediate : MonoBehaviour, ILoadStateTrigger
    {
        protected TriggerCallback Callback;

        public void SetCallback(TriggerCallback cb)
        {
            Callback = cb;
        }

        public void Execute()
        {
            Callback(this, LoadStateMarker.State.Immediate);
            Destroy(this);
        }

    }
}
