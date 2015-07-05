using System;
using NSubstitute;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class ConfigNodeSerializerTests
    {
        [Theory, AutoDomainData]
        public void ConfigNodeSerializer_WithNullParameters_Throws(ISerializerSelector selector, IGetFieldInfo fieldQuery)
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null, fieldQuery));
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(selector, null));
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null, null));
        }


        [Fact()]
        public void DeserializeTest()
        {
            Assert.True(false, "not implemented yet");
        }


        [Theory, AutoDomainData] 
        public void Serialize_UsesSelector_ToSerialize_SimpleType(ConfigNode config, string data)
        {
            var surrogate = Substitute.For<ISerializationSurrogate<string>>();
            var selector = Substitute.For<ISerializerSelector>();

            selector
                .GetSerializer(Arg.Is<Type>(t => t == data.GetType()))
                .Returns(Maybe<ISerializer>.With(surrogate));

            var fieldQuery = Substitute.For<IGetFieldInfo>();
            var sut = new ConfigNodeSerializer(selector, fieldQuery);


            sut.Serialize(data, config);


            fieldQuery.DidNotReceiveWithAnyArgs();
            selector.Received(1).GetSerializer(Arg.Is<Type>(t => t == data.GetType()));

            surrogate.Received(1)
                .Serialize(
                    Arg.Is<object>(data), 
                    Arg.Any<string>(), 
                    Arg.Is(config),
                    Arg.Is<IConfigNodeSerializer>(sut));
        }


        [Theory, AutoDomainData]
        public void Serialize_WithSimpleTestObject_ProducesCorrectResults(ConfigNodeSerializer sut, ConfigNode config, SimplePersistentObject simple)
        {
            sut.Serialize(simple, config);

            Assert.True(config.HasData);
            Assert.True(config.nodes.Count == 0);
            Assert.True(config.GetValues().Length == 1); // one persistent field
            Assert.True(config.HasValue("PersistentField"));
            Assert.NotEmpty(config.GetValue("PersistentField"));
        }


        [Theory, AutoDomainData]
        public void Serialize_UsingNativeSerializer_Calls_IReeperPersistentOnSave_SuppliesSeparateConfigNode(ConfigNodeSerializer sut, ConfigNode config)
        {
            var target = Substitute.For<IReeperPersistent>();

            sut.Serialize(target, config);

            target.Received(1).Serialize(Arg.Is(sut), Arg.Is<ConfigNode>(param => !ReferenceEquals(param, config))); // IReeperPersistent should have its OWN node
            target.DidNotReceive().Deserialize(Arg.Any<IConfigNodeSerializer>(), Arg.Any<ConfigNode>());
        }
    }
}
