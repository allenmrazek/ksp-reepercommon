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
    public class GameEventSubscriber_Test
    {
        private static class GameEventSubscriberFactory
        {
            public static GameEventSubscriber<int> Create(IEventData<int> evt)
            {
                return new GameEventSubscriber<int>(evt);
            }
        }



        [Fact]
        void Constructor_ThrowsArgumentNullException_OnNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new GameEventSubscriber<int>(null));
        }


        [Fact]
        void Constructor_SubscribesTo_ProvidedEventSource()
        {
            var evt = Substitute.For<IEventData<int>>();
            var sut = GameEventSubscriberFactory.Create(evt);

            evt.Received().Add(Arg.Is<Action<int>>(sut.OnEvent));
        }


        [Fact]
        void Dispose_UnsubscribesFrom_ProvidedEventSource()
        {
            var evt = Substitute.For<IEventData<int>>();
            var sut = GameEventSubscriberFactory.Create(evt);

            sut.Dispose();

            evt.Received().Remove(Arg.Is<Action<int>>(sut.OnEvent));
        }
    }
}
