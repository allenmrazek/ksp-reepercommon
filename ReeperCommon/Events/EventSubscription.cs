using System;
using ReeperCommon.Extensions;

namespace ReeperCommon.Events
{
    public class EventSubscription<T> : IEventSubscription
    {
        private readonly Action<T> _callback;
        private readonly WeakReference _sourceReference;



        public EventSubscription(IEventSubscriber<T> src, Action<T> callback)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (callback == null) throw new ArgumentNullException("callback");

            _callback = callback;
            _sourceReference = new WeakReference(src);
        }



        public void Dispose()
        {
            Dispose(true);
        }



        ~EventSubscription()
        {
            Dispose(false);
        }



        private void Dispose(bool disposing)
        {
            if (disposing && _sourceReference.IsAlive)
            {
                var src = _sourceReference.Target as IEventSubscriber<T>;

                if (src.IsNull())
                    throw new InvalidOperationException("Could not cast WeakReference to " +
                                                        typeof (IEventSubscriber<T>).FullName);

                src.RemoveListener(_callback);
            }

            GC.SuppressFinalize(this);
        }
    }
}
