using System;

namespace ReeperCommon.Events
{
    public interface IGameEventSubscriber<T> : IDisposable
    {
        IGameEventSubscription AddListener(Action<T> callback);
        void RemoveListener(Action<T> callback);

        void OnEvent(T arg);
    }
}
