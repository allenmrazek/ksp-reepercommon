using System;

namespace ReeperCommon.Events.Implementations
{
    public class EventSubscriber<T> : IEventSubscriber<T>
    {
        private Action<T> _actions = delegate(T arg) { };



        public IEventSubscription AddListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            _actions += callback;

            return new EventSubscription<T>(this, callback);
        }



        public void RemoveListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            // ReSharper disable once DelegateSubtraction
            _actions -= callback;
        }




        public void OnEvent(T arg)
        {
            _actions(arg);
        }
    }
}
