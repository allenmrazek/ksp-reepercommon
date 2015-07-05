using System;
using System.Linq;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Surrogates;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization.Surrogates
{
    public class PrimitiveSurrogateTests
    {
        [Theory, AutoDomainData]
        public void Serialize_With_NullData_ThrowsException(PrimitiveSurrogate sut, ConfigNode config, string key)
        {
            Assert.Throws<ArgumentNullException>(
                () => sut.Serialize(null, key, config, Substitute.For<IConfigNodeSerializer>()));
        }


        [Theory, AutoDomainData]
        public void Deserialize_With_NullData_ThrowsException(PrimitiveSurrogate sut, ConfigNode config, string key)
        {
            throw new NotImplementedException();
        }
    }


    public abstract class PrimitiveSurrogateTests<T>
    {
        [Theory, AutoDomainData]
        public void Serialize_With_SupportedTypes_AddsSingleKeyToConfigNode(PrimitiveSurrogate sut, T data, ConfigNode config, string key)
        {
            sut.Serialize(data, key, config, Substitute.For<IConfigNodeSerializer>());

            Assert.True(config.HasData);
            Assert.True(config.HasValue(key));
        }


        [Fact()]
        public void DeserializeTest()
        {
            throw new NotImplementedException();
        }


        [Theory, AutoDomainData]
        public void GetSupportedTypes_ReturnsCorrectResults(PrimitiveSurrogate sut)
        {
            var result = sut.GetSupportedTypes().ToList();

            Assert.NotEmpty(result);
            Assert.Contains(typeof (T), result);
        }
    }


    public class StringSurrogate : PrimitiveSurrogateTests<string>
    {
    }


    public class IntSurrogate : PrimitiveSurrogateTests<int>
    {
    }


    public class FloatSurrogate : PrimitiveSurrogateTests<float>
    {
    }
}
