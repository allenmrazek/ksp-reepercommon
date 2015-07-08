using System;
using System.Linq;
using System.Runtime.Serialization;
using NSubstitute;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class DefaultSerializerSelectorTests
    {
        private interface GenericInterface<T>
        {
            T SomeProperty { get; }
        }

        [Fact()]
        public void DefaultSerializerSelectorTest()
        {
            Assert.DoesNotThrow(() => new DefaultConfigNodeItemSerializerSelector());
            Assert.DoesNotThrow(() => new DefaultConfigNodeItemSerializerSelector(null));
            Assert.DoesNotThrow(() => new DefaultConfigNodeItemSerializerSelector(Substitute.For<ISurrogateProvider>()));
        }

        [Theory, AutoDomainData]
        public void GetSerializer_Returns_None_IfNoSerializersExist(DefaultConfigNodeItemSerializerSelector sut, string data)
        {
            var result = sut.GetSerializer(typeof (DataAdapterDataAttribute));

            Assert.Empty(result);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Returns_NativeSerializer(DefaultConfigNodeItemSerializerSelector sut)
        {
            var nativeType = Substitute.For<IReeperPersistent>();

            var result = sut.GetSerializer(nativeType.GetType());

            Assert.NotEmpty(result);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Prefers_NativeSerializer_Over_Surrogate(DefaultConfigNodeItemSerializerSelector sut, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var nativeType = Substitute.For<IReeperPersistent>();
            var surrogate = Substitute.For<ISurrogateSerializer>();

            sut.AddSurrogate(nativeType.GetType(), surrogate);

            var result = sut.GetSerializer(nativeType.GetType());

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ReeperPersistentMethodCaller>(result.Single());
            Assert.IsAssignableFrom<INativeSerializer>(
                ((ReeperPersistentMethodCaller)result.Single()).DecoratedSerializer);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Uses_Surrogate_IfTypeDoesntHaveNativeSerializer(DefaultConfigNodeItemSerializerSelector sut, ISurrogateSerializer<string> surrogateSerializer, string data)
        {
            sut.AddSurrogate(surrogateSerializer);

            var result = sut.GetSerializer(typeof (string));

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ReeperPersistentMethodCaller>(result.Single());
            Assert.IsAssignableFrom<ISurrogateSerializer>(
                ((ReeperPersistentMethodCaller) result.Single()).DecoratedSerializer);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Uses_GenericSurrogate_IfOneSetAndNoOtherSurrogateMatches(
            DefaultConfigNodeItemSerializerSelector sut, string data)
        {
            var genericSurrogateInstance = Substitute.For<ISurrogateSerializer>();
            sut.AddSurrogate(typeof (GenericInterface<>), genericSurrogateInstance);

            var actual = sut.GetSerializer(typeof (GenericInterface<float>));

            Assert.NotEmpty(actual);
            Assert.True(actual.Single() is ReeperPersistentMethodCaller);
            Assert.Same(genericSurrogateInstance, ((ReeperPersistentMethodCaller)actual.Single()).DecoratedSerializer);
        }
    }
}
