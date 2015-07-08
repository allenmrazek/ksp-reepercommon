using System;
using System.Linq;
using NSubstitute;
using NSubstitute.Core;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class NativeSerializerTests
    {
        private class DefaultConstructableType : IReeperPersistent
        {
            public DefaultConstructableType()
            {
                
            }

            public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
            {
                
            }

            public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
            {

            }
        }


        [Theory, AutoDomainData]
        public void Serialize_WithNullParameters_Throws(string key, ConfigNode config)
        {
            object testObject = Substitute.For<IReeperPersistent>();
            var serializer = Substitute.For<IConfigNodeSerializer>();
            var sut = new NativeSerializer();

            Assert.Throws<ArgumentNullException>(() => sut.Serialize(null, testObject, key, config, serializer));
            Assert.DoesNotThrow(() => sut.Serialize(testObject.GetType(), testObject, key, config, serializer)); // null target shouldn't throw
            Assert.Throws<ArgumentNullException>(
                () => sut.Serialize(testObject.GetType(), testObject, null, config, serializer));
            Assert.Throws<ArgumentNullException>(
                () => sut.Serialize(testObject.GetType(), testObject, key, null, serializer));
            Assert.Throws<ArgumentNullException>(
                () => sut.Serialize(testObject.GetType(), testObject, key, config, null));
        }


        [Theory, AutoDomainData]
        public void Serialize_WithTargetTypeThatDoesNotMatchSpecifiedType_Throws(string key, ConfigNode config,
            IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IReeperPersistent>();
            var sut = new NativeSerializer();
            var wrong = key;

            Assert.Throws<WrongNativeSerializerException>(
                () => sut.Serialize(testObject.GetType(), wrong, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Serialize_CallsIReeperPersistent_Serialize_WithNewNode(NativeSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IReeperPersistent>();

            sut.Serialize(testObject.GetType(), testObject, key, config, serializer);

            testObject.Received(1).Serialize(Arg.Is(serializer), Arg.Is<ConfigNode>(cfg => !ReferenceEquals(cfg, config) && config.GetNodes().Any(n => ReferenceEquals(n, cfg))));
        }


        [Theory, AutoDomainData]
        public void Serialize_WithTypeThatIsNotIReeperPersistent_Throws(NativeSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IPersistenceSave>();

            Assert.Throws<ArgumentException>(() => sut.Serialize(testObject.GetType(), testObject, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_CallsIReeperPersistent_Deserialize_WithNewNode(NativeSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IReeperPersistent>();
            config.AddNode(key);

            var result = sut.Deserialize(testObject.GetType(), testObject, key, config, serializer);

            testObject.Received(1).Deserialize(Arg.Is(serializer), Arg.Is<ConfigNode>(cfg => !ReferenceEquals(cfg, config) && config.GetNodes().Any(n => ReferenceEquals(n, cfg))));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithNoConfigValue_ReturnsUnchangedTarget(NativeSerializer sut, string key,
            ConfigNode config, IConfigNodeSerializer serializer)
        {
            var expected = Substitute.For<IReeperPersistent>();

            var actual = sut.Deserialize(expected.GetType(), expected, key, config, serializer);

            Assert.Same(expected, actual);
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithNullTarget_WithReferenceType_ThrowsWhenNoDefaultConstructor(NativeSerializer sut,
            string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var expectedType = Substitute.For<IReeperPersistent>();
            config.AddNode(key);

            Assert.Throws<NoDefaultValueException>(() => sut.Deserialize(expectedType.GetType(), null, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithNullTarget_WithDifferentType_Throws(NativeSerializer sut,
            string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var expectedType = Substitute.For<IReeperPersistent>();
            var badType = Substitute.For<IPersistenceLoad>();

            config.AddNode(key);


            Assert.Throws<WrongNativeSerializerException>(
// ReSharper disable once ExpressionIsAlwaysNull
                () => sut.Deserialize(expectedType.GetType(), badType, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithTypeThatIsNotIReeperPersistent_Throws(NativeSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IConfigNode>();

            config.AddNode(key);

            Assert.Throws<ArgumentException>(() => sut.Deserialize(testObject.GetType(), testObject, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Deserialize_WithNullReferenceType_CreatesNewInstance(NativeSerializer sut, string key,
            ConfigNode config, IConfigNodeSerializer serializer)
        {
            config.AddNode(key);

            var actual = sut.Deserialize(typeof (DefaultConstructableType), null, key, config, serializer);

            Assert.NotNull(actual);
            Assert.Same(actual.GetType(), typeof (DefaultConstructableType));
            Assert.True(actual is IReeperPersistent);
        }

        // todo: make sure all types in this assembly which implement native serializers pass
    }


}
