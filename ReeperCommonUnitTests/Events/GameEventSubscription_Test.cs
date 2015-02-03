using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using ReeperCommon.Events;
using Xunit;
using ReeperCommon.Events.Implementations;

namespace ReeperCommonUnitTests.Events
{
    public class GameEventSubscription_Test
    {
        [Fact]
        void Constructor_NullArgCheck()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new GameEventSubscription<string>(null,
                    Substitute.For<Action<string>>()));

            Assert.Throws<ArgumentNullException>(() =>
                new GameEventSubscription<string>(
                    Substitute.For<IGameEventSubscriber<string>>(),
                    null));

        }


        [Fact]
        void Dispose_RemovesListener_From_Source()
        {
            Action<string> callback = a => { };

            var shouldReceiveRemoveListener = Substitute.For<IGameEventSubscriber<string>>();
            var sut = new GameEventSubscription<string>(shouldReceiveRemoveListener, callback);


            sut.Dispose();


            shouldReceiveRemoveListener.Received(1).RemoveListener(Arg.Is(callback));
        }
    }
}
