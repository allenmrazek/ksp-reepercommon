using System;

namespace ReeperCommon.Events.Implementations
{
    public class GameEventSubscription<T> : IGameEventSubscription
    {
        private readonly IGameEventSubscriber<T> _src;
        private readonly Action<T> _callback;



        public GameEventSubscription(IGameEventSubscriber<T> src, Action<T> callback)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (callback == null) throw new ArgumentNullException("callback");

            _src = src;
            _callback = callback;
        }



        public void Dispose()
        {
            Dispose(true);
        }



        ~GameEventSubscription()
        {
            Dispose(false);
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _src.RemoveListener(_callback);
            }
        }
    }
}
