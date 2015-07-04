using System.Reflection;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace ReeperCommonUnitTests.Serialization
{
    public class ConfigNodeSerializerTests
    {
        [Fact()]
        public void ConfigNodeSerializerTest()
        {
            Assert.True(false, "not implemented yet");
        }

        [Fact()]
        public void DeserializeTest()
        {
            Assert.True(false, "not implemented yet");
        }

        [Theory, AutoDomainData]
        public void SerializeTest(ConfigNodeSerializer sut, ConfigNode config)
        {
            // todo: rewrite this from scratch
            var target = Substitute.For<IReeperPersistent>();
            var surrogate = Substitute.For<ISerializationSurrogate>();

            
            sut.Serialize(target, config);


            Assert.True(config.HasData);
            Assert.True(config.nodes.Count == 0); // shouldn't be any nodes for a simple object
        }
    }
}
