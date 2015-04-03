using System;
using NSubstitute;
using ReeperCommon.Events;
using Xunit;

namespace ReeperCommonUnitTests.Events
{
    public class EventSubscription_Test
    {
        [Fact]
        void Constructor_NullArgCheck()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new EventSubscription<string>(null,
                    (s) => { }));

            Assert.Throws<ArgumentNullException>(() =>
                new EventSubscription<string>(
                    Substitute.For<IEventSubscriber<string>>(),
                    null));

        }


        [Fact]
        void Dispose_RemovesListener_From_Source()
        {
            Action<string> callback = a => { };

            var shouldReceiveRemoveListener = Substitute.For<IEventSubscriber<string>>();

         
            var sut = new EventSubscription<string>(shouldReceiveRemoveListener, callback);


            sut.Dispose();


            shouldReceiveRemoveListener.Received(1).RemoveListener(Arg.Is(callback));
        }
    }
}
