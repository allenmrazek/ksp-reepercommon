using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Containers;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class ConfigNodeFormatterTests
    {
        [Theory, AutoDomainData]
        public void Serialize_SimplePersistentObject([Frozen] SimplePersistentObject testObject, ConfigNode config, [Frozen] SerializableFieldQuery fieldQuery)
        {
            var selector = Substitute.For<ISurrogateSelector>();

            selector.GetSurrogate(Arg.Any<Type>()).Returns(Maybe<ISerializationSurrogate>.With(Substitute.For<ISerializationSurrogate>()));

            var sut = new ConfigNodeSerializer(selector, fieldQuery);

            sut.Serialize(testObject, config);

            Assert.NotEmpty(selector.ReceivedCalls());
        }


        [Theory, AutoDomainData]
        public void Serialize_SimplePersistentObject_Real([Frozen] ComplexPersistentObject testObject, ConfigNode config,
            [Frozen] SerializableFieldQuery fieldQuery)
        {
            var formatter = new ConfigNodeSerializer(new DefaultSurrogateSelector(new DefaultSurrogateProvider()), new SerializableFieldQuery());

            formatter.Serialize(testObject, config);

            Assert.True(config.HasValue("MyTestString"));
            //Assert.True(config.HasNode("
            config.Save("D:/testConfig.cfg");

            Assert.True(config.HasData);
        }




        [Theory, AutoDomainData]
        public void Deserialize_SimplePersistentObject([Frozen] SimplePersistentObject testObject, ConfigNode config)
        {
            var surrogate = Substitute.For<ISerializationSurrogate>();
            var selector = Substitute.For<ISurrogateSelector>();

            selector.GetSurrogate(Arg.Any<Type>())
                .Returns(Maybe<ISerializationSurrogate>.With(surrogate));

            var sut = new ConfigNodeSerializer(selector, new SerializableFieldQuery());

            sut.Deserialize(testObject, config);

            surrogate.Received(1)
                .Deserialize(
                    Arg.Any<object>(),
                    Arg.Any<FieldInfo>(),
                    Arg.Is<ConfigNode>(arg => ReferenceEquals(config, arg)),
                    sut);

            selector.Received(1).GetSurrogate(typeof(string));
        }


        [Theory, AutoDomainData]
        public void Serialize_ComplexPersistentObject_EnsureIReeperPersistent_Save_MethodUsed(
            ComplexPersistentObject testObject, ConfigNode config, SerializableFieldQuery fieldQuery)
        {
            var mockedPersist = Substitute.For<IReeperPersistent>();
            var surrogateSelector = Substitute.For<ISurrogateSelector>();

            surrogateSelector.GetSurrogate(Arg.Any<Type>())
                .Returns(Maybe<ISerializationSurrogate>.With(Substitute.For<ISerializationSurrogate>()));

            testObject.MyTestObject = mockedPersist;

            var sut = new ConfigNodeSerializer(surrogateSelector, fieldQuery);

            sut.Serialize(testObject, config);

            mockedPersist.Received(1).Serialize(sut, Arg.Is<ConfigNode>(node => config.nodes.GetNodes().Contains(node)));
        }



        [Theory, AutoDomainData]
        public void Deserialize_ComplexPersistentObject_EnsureIReeperPersistent_Load_MethodUsed(
            ComplexPersistentObject testObject, ConfigNode config, SerializableFieldQuery fieldQuery)
        {
            var mockedPersist = Substitute.For<IReeperPersistent>();
            var surrogateSelector = Substitute.For<ISurrogateSelector>();

            surrogateSelector.GetSurrogate(Arg.Any<Type>())
                .Returns(Maybe<ISerializationSurrogate>.With(Substitute.For<ISerializationSurrogate>()));

            testObject.MyTestObject = mockedPersist;

            var sut = new ConfigNodeSerializer(surrogateSelector, fieldQuery);

            config.AddNode("MyTestObject"); // contents irrelevant

            sut.Deserialize(testObject, config);

            mockedPersist.Received(1).Deserialize(sut, Arg.Is<ConfigNode>(node => config.nodes.GetNodes().Contains(node)));
        }




        [Fact]
        public void ConstructorThrows_OnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null, Substitute.For<IFieldInfoQuery>()));
            Assert.Throws<ArgumentNullException>(
                () => new ConfigNodeSerializer(Substitute.For<ISurrogateSelector>(), null));
        }


        [Theory, AutoDomainData]
        public void Serialize_Throws_OnNull([Frozen] SimplePersistentObject testObject, ConfigNode config)
        {
            var sut = new ConfigNodeSerializer(Substitute.For<ISurrogateSelector>(), Substitute.For<IFieldInfoQuery>());

            Assert.Throws<ArgumentNullException>(() => sut.Serialize(testObject, null));
            Assert.Throws<ArgumentNullException>(() => sut.Serialize(null, config));
        }


        [Theory, AutoDomainData]
        public void Deserialize_Throws_OnNull([Frozen] SimplePersistentObject targetObject, ConfigNode config)
        {
            var sut = new ConfigNodeSerializer(Substitute.For<ISurrogateSelector>(), Substitute.For<IFieldInfoQuery>());

            Assert.Throws<ArgumentNullException>(() => sut.Deserialize(null, config));
            Assert.Throws<ArgumentNullException>(() => sut.Deserialize(targetObject, null));
        }
    }
}
