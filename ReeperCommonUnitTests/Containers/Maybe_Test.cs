using System.Linq;
using Xunit;

namespace ReeperCommonUnitTests.Containers
{
    public class Maybe_Test
    {
        [Fact]
        void None_Returns_Empty()
        {
            Assert.Empty(ReeperCommon.Containers.Maybe<string>.None);
            Assert.False(ReeperCommon.Containers.Maybe<string>.None.Any());
        }

        [Fact]
        void With_Returns_NotEmpty()
        {
            Assert.NotEmpty(ReeperCommon.Containers.Maybe<string>.With("test"));
            Assert.True(ReeperCommon.Containers.Maybe<string>.With("test").Any());
        }
    }
}
