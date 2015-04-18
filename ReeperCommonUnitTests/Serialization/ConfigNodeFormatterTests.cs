using System;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using ReeperCommonUnitTests.Fixtures.AssemblyReloaderTests.FixtureCustomizations;
using ReeperCommonUnitTests.TestData;
using UnityEngine;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class ConfigNodeFormatterTests
    {
        [Fact()]
        public void DeserializeTest()
        {
            Assert.True(false, "not implemented yet");
        }


        [Theory, AutoDomainData]
        public void SerializeTest_SimpleObject([Frozen] SimplePersistentObject testObject, ConfigNode config)
        {
            var selector = Substitute.For<ISurrogateSelector>();
            var sut = new ConfigNodeFormatter(selector, new SerializableFieldQuery());

            var result = sut.Serialize(testObject, config);


            Assert.True(result, "Serialization failed");
            Assert.Empty(selector.ReceivedCalls()); // selector shouldn't be used for this test because of basic KSP serializable types
            Assert.True(config.HasData);
            Assert.True(
                new SerializableFieldQuery().Get(testObject)
                    .All(fieldInfo =>
                        config.HasValue(fieldInfo.Name)));
        }


        [Fact]
        public void ConstructorThrows_OnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeFormatter(null, Substitute.For<IFieldInfoQuery>()));
            Assert.Throws<ArgumentNullException>(
                () => new ConfigNodeFormatter(Substitute.For<ISurrogateSelector>(), null));
        }


        [Theory, AutoDomainData]
        public void Serialize_Throws_OnNull([Frozen] SimplePersistentObject testObject, ConfigNode config)
        {
            var sut = new ConfigNodeFormatter(Substitute.For<ISurrogateSelector>(), Substitute.For<IFieldInfoQuery>());

            Assert.Throws<ArgumentNullException>(() => sut.Serialize(testObject, null));
            Assert.Throws<ArgumentNullException>(() => sut.Serialize(null, config));
        }
    }
}
