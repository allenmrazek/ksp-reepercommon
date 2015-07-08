using System;
using System.Linq;
using Ploeh.AutoFixture;
using ReeperCommon.Containers;
using Xunit;

namespace ReeperCommonUnitTests.Containers
{
// ReSharper disable once InconsistentNaming
    public class Maybe_Test
    {
        [Fact]
        void None_Returns_Empty()
        {
            Assert.Empty(Maybe<string>.None);
            Assert.False(Maybe<string>.None.Any());
        }

        [Fact]
        void With_Returns_NotEmpty()
        {
            Assert.NotEmpty(Maybe<string>.With("test"));
            Assert.True(Maybe<string>.With("test").Any());
        }

        [Fact]
        void Or_Maybe_Returns_First_IfNotEmpty()
        {
            var first = "first".ToMaybe();
            var second = "second";
            var emptyFirst = Maybe<string>.None;

            var expectedFirst = first.Or(second);
            var expectedSecond = emptyFirst.Or(second);

            Assert.Same(expectedFirst, first.Single());
            Assert.Same(expectedSecond, second);
        }

        [Fact]
        void Or_T_Returns_First_IfNotEmpty()
        {
            var first = "first".ToMaybe();
            var emptyFirst = Maybe<string>.None;
            var second = Maybe<string>.With("second");

            var expectedFirst = first.Or(second);
            var expectedSecond = emptyFirst.Or(second);

            Assert.Same(expectedFirst.Single(), first.Single());
            Assert.Same(expectedSecond.Single(), second.Single());
        }

        [Fact]
        void ToMaybe_Converts_References_ToMaybe()
        {
            var expectedValue = "first".ToMaybe();
            var expectedNoValue = (null as string).ToMaybe();

            Assert.True(expectedValue.Any());
            Assert.False(expectedNoValue.Any());
        }


        [Fact]
        void ToMaybe_Converts_ValueTypes_ToMaybe()
        {
            var fixture = new Fixture();

            var values = fixture.CreateMany<float>(3);

            foreach (var expected in values.Select(v => v.ToMaybe()))
                Assert.True(expected.Any());
        }
    }
}
