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
        public void Deserialize_CallsIReeperPersistent_Deserialize_WithNewNode(NativeSerializer sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var testObject = Substitute.For<IReeperPersistent>();
            config.AddNode(key);

            var result = sut.Deserialize(testObject.GetType(), testObject, key, config, serializer);

            testObject.Received(1).Deserialize(Arg.Is(serializer), Arg.Is<ConfigNode>(cfg => !ReferenceEquals(cfg, config) && config.GetNodes().Any(n => ReferenceEquals(n, cfg))));
        }


        // todo: make sure all types in this assembly which implement native serializers pass
    }


}
