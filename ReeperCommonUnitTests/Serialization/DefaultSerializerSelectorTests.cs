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

        private class GenericSerializer<T> : IConfigNodeItemSerializer
        {
            public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        private class GenericObject<T>
        {
            
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
            var surrogate = Substitute.For<IConfigNodeItemSerializer>();

            sut.AddSerializer(nativeType.GetType(), surrogate);

            var result = sut.GetSerializer(nativeType.GetType());

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ReeperPersistentMethodCaller>(result.Single());
            Assert.IsAssignableFrom<NativeSerializer>(
                ((ReeperPersistentMethodCaller)result.Single()).DecoratedSerializer);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Uses_Surrogate_IfTypeDoesntHaveNativeSerializer(DefaultConfigNodeItemSerializerSelector sut, IConfigNodeItemSerializer surrogateSerializer, string data)
        {
            sut.AddSerializer<string>(surrogateSerializer);

            var result = sut.GetSerializer(typeof(string));

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ReeperPersistentMethodCaller>(result.Single());
            Assert.IsAssignableFrom<IConfigNodeItemSerializer>(
                ((ReeperPersistentMethodCaller)result.Single()).DecoratedSerializer);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Uses_GenericSurrogate_IfOneSetAndNoOtherSurrogateMatches(
            DefaultConfigNodeItemSerializerSelector sut, string data)
        {
            var genericSurrogateInstance = Substitute.For<IConfigNodeItemSerializer>();
            sut.AddSerializer(typeof(GenericInterface<>), genericSurrogateInstance);

            var actual = sut.GetSerializer(typeof(GenericInterface<float>));

            Assert.NotEmpty(actual);
            Assert.True(actual.Single() is ReeperPersistentMethodCaller);
            Assert.Same(genericSurrogateInstance, ((ReeperPersistentMethodCaller)actual.Single()).DecoratedSerializer);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_UsesGenericFactory_IfNoNativeOrNonGenericTypeDefinitionsExist(
            DefaultConfigNodeItemSerializerSelector sut, string data)
        {
            var expected = Substitute.For<IConfigNodeItemSerializer>();

            sut.AddSerializer(typeof (GenericObject<>), t => Maybe<IConfigNodeItemSerializer>.With(expected));

            var actual = sut.GetSerializer(typeof (GenericObject<float>));

            // note: returned result is (should be) wrapped by ReeperPersistentMethodCaller
            Assert.NotEmpty(actual);
            Assert.True(actual.Single() is ReeperPersistentMethodCaller);
            Assert.Same(expected, ((ReeperPersistentMethodCaller)actual.Single()).DecoratedSerializer);
        }
    }
}
