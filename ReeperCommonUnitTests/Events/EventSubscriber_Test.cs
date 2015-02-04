using ReeperCommon.Events.Implementations;

namespace ReeperCommonUnitTests.Events
{
    public class EventSubscriber_Test
    {
        private static class GameEventSubscriberFactory
        {
            public static EventSubscriber<int> Create()
            {
                return new EventSubscriber<int>();
            }
        }
    }
}
