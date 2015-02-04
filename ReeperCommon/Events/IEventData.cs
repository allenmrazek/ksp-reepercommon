using System;

namespace ReeperCommon.Events
{
    public interface IEventData<T>
    {
        IEventSubscription Add(Action<T> evt);
        void Remove(Action<T> evt);
        void Fire(T value);
    }
}
