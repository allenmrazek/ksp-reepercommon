using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using ReeperCommon.Events;
using ReeperCommon.Events.Implementations;
using Xunit;

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
