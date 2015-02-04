using System;

namespace ReeperCommon.Events
{
    public interface IEventSubscriber<T>
    {
        IEventSubscription AddListener(Action<T> callback);
        void RemoveListener(Action<T> callback);

        void OnEvent(T arg);
    }
}
