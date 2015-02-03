using System;

namespace ReeperCommon.Events.Implementations
{
    public class KSPEventData<T> : IEventData<T>
    {
        private readonly EventData<T> _eventData;

        public KSPEventData(EventData<T> eventData)
        {
            if (eventData == null) throw new ArgumentNullException("eventData");
            _eventData = eventData;
        }

        public void Add(Action<T> evt)
        {
            _eventData.Add(new EventData<T>.OnEvent(evt));
        }

        public void Remove(Action<T> evt)
        {
            _eventData.Remove(new EventData<T>.OnEvent(evt));
        }

        public void Fire(T value)
        {
            _eventData.Fire(value);
        }
    }
}
