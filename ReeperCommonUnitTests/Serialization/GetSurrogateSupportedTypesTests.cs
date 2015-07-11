using System;
using System.Linq;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public abstract class GetSurrogateSupportedTypesTests<T>
    {
        [Theory, AutoDomainData]
        public void Get_WithSurrogateThatSupportsSingleType_ReturnsCorrectType(GetSurrogateSupportedTypes sut)
        {
            var surrogate = Substitute.For<IConfigNodeItemSerializer<T>>();

            var actual = sut.Get(surrogate.GetType()).ToList();


            Assert.NotEmpty(actual);
            Assert.True(actual.Single() == typeof(T));
        }


        [Theory, AutoDomainData]
        public void Get_WithSurrogateThatSupportsMultipleTypes_ReturnsAllSupportedTypes(GetSurrogateSupportedTypes sut)
        {
            var surrogate = Substitute.For<IConfigNodeItemSerializer<T>, IConfigNodeItemSerializer<int>, IConfigNodeItemSerializer<ConfigNode>>();

            var actual = sut.Get(surrogate.GetType()).ToList();

            Assert.NotEmpty(actual);
            Assert.Contains(typeof(int), actual);
            Assert.Contains(typeof(ConfigNode), actual);
            Assert.Contains(typeof(T), actual);
        }
    }


    public class StringSurrogateTests : GetSurrogateSupportedTypesTests<string>
    {
    }


    public class FloatSurrogateTests : GetSurrogateSupportedTypesTests<float>
    {
    }


    public class ConfigNodeSurrogateTests : GetSurrogateSupportedTypesTests<ConfigNode>
    {
    }
}
