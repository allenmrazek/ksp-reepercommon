using System;
using System.Linq;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class DefaultSerializerSelectorTests
    {
        [Fact()]
        public void DefaultSerializerSelectorTest()
        {
            Assert.DoesNotThrow(() => new DefaultSerializerSelector());
            Assert.DoesNotThrow(() => new DefaultSerializerSelector(null));
            Assert.DoesNotThrow(() => new DefaultSerializerSelector(Substitute.For<ISurrogateProvider>()));
        }

        [Theory, AutoDomainData]
        public void GetSerializer_Returns_None_IfNoSerializersExist(DefaultSerializerSelector sut, string data)
        {
            var result = sut.GetSerializer(typeof (DataAdapterDataAttribute));

            Assert.Empty(result);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Returns_NativeSerializer(DefaultSerializerSelector sut)
        {
            var nativeType = Substitute.For<IReeperPersistent>();

            var result = sut.GetSerializer(nativeType.GetType());

            Assert.NotEmpty(result);
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Prefers_NativeSerializer_Over_Surrogate(DefaultSerializerSelector sut)
        {
            var nativeType = Substitute.For<IReeperPersistent>();
            sut.AddSurrogate(nativeType.GetType(), Substitute.For<ISerializationSurrogate>());

            var result = sut.GetSerializer(nativeType.GetType());

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ISerializationNative>(result.Single());
        }


        [Theory, AutoDomainData]
        public void GetSerializer_Uses_Surrogate_IfTypeDoesntHaveNativeSerializer(DefaultSerializerSelector sut, ISerializationSurrogate<string> surrogate, string data)
        {
            sut.AddSurrogate(surrogate);

            var result = sut.GetSerializer(typeof (string));

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ISerializationSurrogate>(result.Single());
            Assert.Same(surrogate, result.Single());
        }
    }
}
