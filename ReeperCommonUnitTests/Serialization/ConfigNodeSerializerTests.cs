using System;
using NSubstitute;
using Ploeh.AutoFixture;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class ConfigNodeSerializerTests
    {
        private class NonserializableObject
        {
            private class NonserializableField
            {
            }

            [ReeperPersistent] private NonserializableField _field = new NonserializableField();
        }


        [Theory, AutoDomainData]
        public void ConfigNodeSerializer_WithNullParameters_Throws(IConfigNodeItemSerializerSelector selector, IGetObjectFields fieldQuery)
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null, fieldQuery));
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(selector, null));
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null, null));
        }


        [Theory, AutoDomainData] 
        public void Serialize_UsesSelector_ToSerialize_SimpleType(ConfigNode config, string data)
        {
            var surrogate = Substitute.For<ISurrogateSerializer<string>>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();

            selector
                .GetSerializer(Arg.Is<Type>(t => t == data.GetType()))
                .Returns(Maybe<IConfigNodeItemSerializer>.With(surrogate));

            var fieldQuery = Substitute.For<IGetObjectFields>();
            var sut = new ConfigNodeSerializer(selector, fieldQuery);


            sut.Serialize(data, config);


            fieldQuery.DidNotReceiveWithAnyArgs();
            selector.Received(1).GetSerializer(Arg.Is<Type>(t => t == data.GetType()));

            surrogate.Received(1)
                .Serialize(
                    Arg.Is<Type>(type => typeof(string) == type),
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
        public void Serialize_WithComplextestObject_ProducesCorrectResults(ConfigNodeSerializer sut, ConfigNode config,
            ComplexPersistentObject complex)
        {
            sut.Serialize(complex, config);

            Assert.True(config.HasData);
            Assert.True(config.nodes.Count > 0);
            Assert.True(config.values.Count == 5);
        }


        [Theory, AutoDomainData]
        public void Serialize_UsingNativeSerializer_Calls_IReeperPersistentOnSave_SuppliesSeparateConfigNode(ConfigNodeSerializer sut, ConfigNode config)
        {
            var target = Substitute.For<IReeperPersistent>();

            sut.Serialize(target, config);

            target.Received(1).Serialize(Arg.Is(sut), Arg.Is<ConfigNode>(param => !ReferenceEquals(param, config))); // IReeperPersistent should have its OWN node
            target.DidNotReceive().Deserialize(Arg.Any<IConfigNodeSerializer>(), Arg.Any<ConfigNode>());
        }


        [Theory, AutoDomainData]
        public void Deserialize_UsesSelector_ToSerialize_SimpleType(ConfigNode config, string data)
        {
            var surrogate = Substitute.For<ISurrogateSerializer<string>>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();

            selector
                .GetSerializer(Arg.Is<Type>(t => t == data.GetType()))
                .Returns(Maybe<IConfigNodeItemSerializer>.With(surrogate));

            var fieldQuery = Substitute.For<IGetObjectFields>();
            var sut = new ConfigNodeSerializer(selector, fieldQuery);


            sut.Deserialize(data, config);


            fieldQuery.DidNotReceiveWithAnyArgs();
            selector.Received(1).GetSerializer(Arg.Is<Type>(t => t == data.GetType()));

            surrogate.Received(1)
                .Deserialize(
                    Arg.Is<Type>(type => typeof(string) == type),
                    Arg.Is<object>(data),
                    Arg.Any<string>(),
                    Arg.Is(config),
                    Arg.Is<IConfigNodeSerializer>(sut));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithSimpleTestObject_ProducesCorrectResults(ConfigNodeSerializer sut, ConfigNode config,
            SimplePersistentObject expected)
        {
            var fixture = new Fixture();

            var actual = new SimplePersistentObject()
            {
                NonpersistentField = fixture.CreateAnonymous<string>(),
                PersistentField = fixture.CreateAnonymous<string>()
            };

            sut.Serialize(expected, config);
            sut.Deserialize(actual, config);

            Assert.Equal(actual.PersistentField, expected.PersistentField);
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithComplextestObject_ProducesCorrectResults(ConfigNodeSerializer sut, ConfigNode config,
            ComplexPersistentObject complex)
        {
            var fixture = new Fixture();
            fixture.Register(() => (IReeperPersistent)new ComplexPersistentObject.InternalPersistent());

            sut.Serialize(complex, config);
 
            var actual = fixture.CreateAnonymous<ComplexPersistentObject>();

            sut.Deserialize(actual, config);

            Assert.True(config.HasData);
            Assert.True(config.nodes.Count > 0);
            Assert.True(config.values.Count == 5);
            Assert.Equal(actual, complex);
        }


        [Theory, AutoDomainData]
        public void SerializeAndDeserialize_UsingType_ThatHasField_WithNoSerializer_Throws(ConfigNodeSerializer sut, ConfigNode config)
        {
            var notSerializable = new NonserializableObject();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();
            selector.GetSerializer(Arg.Any<Type>()).Returns(ci => Maybe<IConfigNodeItemSerializer>.None);

            sut.ConfigNodeItemSerializerSelector = selector;

            Assert.Throws<NoSerializerFoundException>(() => sut.Serialize(notSerializable, config));
            Assert.Throws<NoSerializerFoundException>(() => sut.Deserialize(notSerializable, config));
        }
    }

}
