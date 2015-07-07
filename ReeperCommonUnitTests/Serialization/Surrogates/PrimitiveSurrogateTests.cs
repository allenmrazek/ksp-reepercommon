using System;
using System.ComponentModel;
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
        public void Serialize_With_NullData_ReturnsWithoutThrowing_And_DoesNotMakeAnyChanges(PrimitiveSurrogateSerializer sut, ConfigNode config, string key)
        {
            Assert.DoesNotThrow(
                () => sut.Serialize(typeof(string), null, key, config, Substitute.For<IConfigNodeSerializer>()));

            Assert.False(config.HasData);
        }


        [Theory, AutoDomainData]
        public void Serialize_With_NullParams_ExceptData_Throws(PrimitiveSurrogateSerializer sut, float data, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Serialize(data.GetType(), data, null, config, serializer));
            Assert.Throws<ArgumentNullException>(() => sut.Serialize(data.GetType(), data, key, null, serializer));
            Assert.Throws<ArgumentNullException>(() => sut.Serialize(data.GetType(), data, key, config, null));
        }


        [Theory, AutoDomainData]
        public void Deserialize_With_NullData_DoesNotThrow(PrimitiveSurrogateSerializer sut, ConfigNode config, string key)
        {
            Assert.DoesNotThrow(
                () => sut.Deserialize(typeof(string), null, key, config, Substitute.For<IConfigNodeSerializer>()));
        }


        [Theory, AutoDomainData]
        public void Deserialize_With_NullParams_ExceptData_Throws(PrimitiveSurrogateSerializer sut, float data, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Deserialize(data.GetType(), data, null, config, serializer));
            Assert.Throws<ArgumentNullException>(() => sut.Deserialize(data.GetType(), data, key, null, serializer));
            Assert.Throws<ArgumentNullException>(() => sut.Deserialize(data.GetType(), data, key, config, null));
        }
    }


    public abstract class PrimitiveSurrogateTests<T>
    {
        [Theory, AutoDomainData]
        public void Serialize_With_SupportedTypes_AddsSingleKeyToConfigNode(PrimitiveSurrogateSerializer sut, T data, ConfigNode config, string key)
        {
            sut.Serialize(typeof(T), data, key, config, Substitute.For<IConfigNodeSerializer>());

            Assert.True(config.HasData);
            Assert.True(config.HasValue(key));
        }


        [Theory, AutoDomainData]
        public void Deserialize_With_SupportedTypes_ResultsInExpectedValue(PrimitiveSurrogateSerializer sut, T data, ConfigNode config)
        {
            var serializer = Substitute.For<IConfigNodeSerializer>();
            var tc = TypeDescriptor.GetConverter(typeof (T));
            config.AddValue("key", tc.ConvertToInvariantString(data));
            var expected = data;


            var actual = (T)sut.Deserialize(typeof(T), expected, "key", config, serializer);
            

            Assert.True(tc.CanConvertTo(typeof (string)));
            serializer.DidNotReceive().Deserialize(Arg.Any<object>(), Arg.Any<ConfigNode>());
            Assert.Equal(expected, actual);
        }


        [Theory, AutoDomainData]
        public void Serializer_With_SupportedTypes_ThenDeserialize_ReturnsEquivalentValues(PrimitiveSurrogateSerializer sut,
            T data, ConfigNode config)
        {
            var serializer = Substitute.For<IConfigNodeSerializer>();
            var expected = data;

            sut.Serialize(typeof(T), data, "key", config, serializer);

            var actual = sut.Deserialize(typeof(T), default(T), "key", config, serializer);


            serializer.DidNotReceive().Serialize(Arg.Any<object>(), Arg.Any<ConfigNode>());
            serializer.DidNotReceive().Deserialize(Arg.Any<object>(), Arg.Any<ConfigNode>());
            var tmp = serializer.DidNotReceive().ConfigNodeItemSerializerSelector;

            Assert.Equal(actual, expected);
            Assert.True(config.HasData);
            Assert.True(config.HasValue("key"));
        }


        [Theory, AutoDomainData]
        public void GetSupportedTypes_ReturnsCorrectResults(PrimitiveSurrogateSerializer sut)
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
