using System;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using ReeperCommon.Serialization.Surrogates;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization.Surrogates
{
    public class ConfigNodeSurrogateSerializerTests
    {
        [Theory, AutoDomainData]
        public void Serialize_AppendsNodeCorrectly(ConfigNodeSurrogateSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var toSerialize = new ConfigNode("Node");
            toSerialize.AddValue("Key", "Value");

            sut.Serialize(typeof(ConfigNode), toSerialize, key, config, serializer);

            Assert.True(config.HasData);
            Assert.True(config.HasNode(key));
            Assert.True(config.nodes[0].HasValue("Key"));
            Assert.True(config.nodes[0].GetValues().Length == 1);
        }


        [Theory, AutoDomainData]
        public void Serialize_IntoConfigWithExistingKey_Throws(ConfigNodeSurrogateSerializer sut, string key,
            ConfigNode config, IConfigNodeSerializer serializer)
        {
            config.AddNode(key);

            Assert.Throws<ConfigNodeDuplicateKeyException>(
                () => sut.Serialize(typeof (ConfigNode), new ConfigNode(), key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Serialize_WithWrongType_Throws(ConfigNodeSurrogateSerializer sut, string key, ConfigNode config,
            IConfigNodeSerializer serializer)
        {
            var badObject = Substitute.For<IReeperPersistent>();

            Assert.Throws<WrongSerializerException>(
                () => sut.Serialize(typeof(ConfigNode), badObject, key, config, serializer));
            Assert.Throws<WrongSerializerException>(
                () => sut.Serialize(badObject.GetType(), new ConfigNode(), key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_DeserializesNodeCorrectly(ConfigNodeSurrogateSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var source = config.AddNode(key);
            source.AddValue("Key", "Value");

            var actual = sut.Deserialize(typeof(ConfigNode), null, key, config, serializer);

            Assert.NotNull(actual);
            Assert.True(actual is ConfigNode);

            var result = actual as ConfigNode;

            Assert.Empty(result.GetNodes());
            Assert.NotEmpty(result.GetValues());
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithTargetThatHasExistingData_DeserializesNodeCorrectly(
            ConfigNodeSurrogateSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var source = config.AddNode(key);
            source.AddValue("Key", "Value");

            // add some garbage to try and confuse the serializer into leaving stuff in
            var existing = new ConfigNode("existingData");

            existing.AddValue("Garbage", "ThisIsGarbage");
            existing.AddNode("GarbageNode").AddValue("MoreGarbage", "Dead");

            var actual = sut.Deserialize(typeof(ConfigNode), existing, key, config, serializer);
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithTargetConfigNodeThatIsSameAsSource_DoesNotModifySource(
            ConfigNodeSurrogateSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            const string dontRemove = "ShouldNotBeRemoved";

            var source = config.AddNode(key);
            source.AddValue("Key", "Value");

            config.AddValue(dontRemove, "DontTouch");

            var actual = sut.Deserialize(typeof (ConfigNode), config, key, config, serializer);

            Assert.NotSame(config, actual);
            Assert.True(config.HasValue(dontRemove));
            Assert.True(config.GetNodes().Length == 1);
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithDuplicateKeys_Throws(ConfigNodeSurrogateSerializer sut, string key,
            ConfigNode config, IConfigNodeSerializer serializer)
        {
            config.AddNode(key);
            config.AddNode(key);

            Assert.Throws<AmbiguousKeyException>(
                () => sut.Deserialize(typeof (ConfigNode), new ConfigNode(), key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithNoExistingKey_WithSameInputConfig_ReturnsNewConfigNode(
            ConfigNodeSurrogateSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var actual = sut.Deserialize(typeof (ConfigNode), config, key, config, serializer);

            Assert.NotSame(actual, config);
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithWrongType_Throws(ConfigNodeSurrogateSerializer sut, string key, ConfigNode config,
            IConfigNodeSerializer serializer)
        {
            config.AddNode(key);
            var badObject = Substitute.For<IReeperPersistent>();

            Assert.Throws<WrongSerializerException>(
                () => sut.Deserialize(typeof (ConfigNode), badObject, key, config, serializer));
            Assert.Throws<WrongSerializerException>(
                () => sut.Deserialize(badObject.GetType(), new ConfigNode(), key, config, serializer));
        }
    }
}
