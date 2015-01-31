﻿using System;
using ReeperCommon.Extensions.Object;
using ReeperCommon.Logging;

namespace ReeperCommon.Events.Implementations
{
    internal class GameEventSubscriber<T> : IGameEventSubscriber<T>
    {

        private readonly ILog _log;
        private WeakReference _subscribed;
        private Action<T> _actions = delegate(T arg) { };


        public GameEventSubscriber(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }




        ~GameEventSubscriber()
        {
            Dispose();
        }



        public virtual void Dispose()
        {
            if (!_subscribed.IsNull() && _subscribed.IsAlive)
            {
                _log.Debug("GameEventSubscriber<" + typeof (T).FullName + "> disposing");
                UnsubscribeTo(_subscribed.Target as EventData<T>);
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
            _log.Debug("Triggered with arg " + arg.ToString());
            _actions(arg);
        }



        public virtual void SubscribeTo(EventData<T> evt)
        {
            if (evt == null) throw new ArgumentNullException("evt");
            if (!_subscribed.IsNull())
                throw new InvalidOperationException("Already subscribed to an event");

            _subscribed = new WeakReference(evt);

            evt.Add(OnEvent);
        }



        public virtual void UnsubscribeTo(EventData<T> evt)
        {
            if (_subscribed.IsAlive)
            {
                var publisher = _subscribed.Target as EventData<T>;

                if (!publisher.IsNull())
                    publisher.Remove(OnEvent);
            }
        }
    }
}
