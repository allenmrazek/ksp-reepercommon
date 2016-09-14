using System;
using JetBrains.Annotations;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace ReeperCommon.Events
{
    public class DisposableEvent<TParam>
    {
        public delegate void EventDelegate(TParam param);

        private class Subscription : IDisposable
        {
            private readonly EventDelegate _del;
            private readonly WeakReference _eventReference;

            public Subscription([NotNull] WeakReference eventReference, [NotNull] EventDelegate del)
            {
                if (del == null) throw new ArgumentNullException("del");
                if (eventReference == null) throw new ArgumentNullException("eventReference");

                _del = del;
                _eventReference = eventReference;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~Subscription()
            {
                Dispose(false);
            }

            private void Dispose(bool disposing)
            {
                if (!_eventReference.IsAlive)
                    return;

                Log.Warning("Disposing of event reference");

                _eventReference.Target.With(t => t as DisposableEvent<TParam>).Do(evt => evt.Event -= _del);
            }
        }

        private event EventDelegate Event = delegate { };

        // ReSharper disable once UnusedMember.Global
        public IDisposable Subscribe([NotNull] EventDelegate del)
        {
            if (del == null) throw new ArgumentNullException("del");

            lock (this)
            {
                Event += del;
                return new Subscription(new WeakReference(this), del);
            }
        }

        public void Fire(TParam param)
        {
            Event(param);
        }
    }
}
