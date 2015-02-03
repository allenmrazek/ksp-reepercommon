using System;
using ReeperCommon.Extensions.Object;

namespace ReeperCommon.Events.Implementations
{
    public class GameEventSubscriber<T> : IGameEventSubscriber<T>
    {
        private readonly WeakReference _subscribed;
        private Action<T> _actions = delegate(T arg) { };


        public GameEventSubscriber(IEventData<T> evt)
        {
            if (evt == null) throw new ArgumentNullException("evt");

            _subscribed = new WeakReference(evt);

            evt.Add(OnEvent);
        }



        ~GameEventSubscriber()
        {
            Dispose();
        }



        public virtual void Dispose()
        {
            if (!_subscribed.IsNull() && _subscribed.IsAlive)
            {
                if (_subscribed.IsAlive)
                {
                    var publisher = _subscribed.Target as IEventData<T>;

                    if (!publisher.IsNull())
                        publisher.Remove(OnEvent);
                }
            }
            GC.SuppressFinalize(this);
        }



        public virtual IGameEventSubscription AddListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            _actions += callback;

            return new GameEventSubscription<T>(this, callback);
        }



        public virtual void RemoveListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            // ReSharper disable once DelegateSubtraction
            _actions -= callback;
        }



        public virtual void OnEvent(T arg)
        {
            _actions(arg);
        }
    }
}
