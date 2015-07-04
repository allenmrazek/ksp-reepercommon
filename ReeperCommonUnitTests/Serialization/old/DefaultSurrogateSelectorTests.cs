using System.Linq;
using NSubstitute;
using Xunit;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class DefaultSurrogateSelectorTests
    {
        [Fact]
        public void AddSurrogate_GenericTest()
        {
            //var sut = new DefaultSerializerSelector();
            //var surrogate = Substitute.For<ISerializationSurrogate<string>>();

            //sut.AddSurrogate(surrogate);

            //var result = sut.GetSurrogate(typeof (string));

            //Assert.True(result.Any());
        }


        [Fact]
        public void AddSurrogate_WithTypeSpecified_Test()
        {
            //var sut = new DefaultSerializerSelector();
            //var surrogate = Substitute.For<ISerializationSurrogate>();

            //sut.AddSurrogate(typeof(string), surrogate);

            //var result = sut.GetSurrogate(typeof(string));

            //Assert.True(result.Any());
        }
    }
}
