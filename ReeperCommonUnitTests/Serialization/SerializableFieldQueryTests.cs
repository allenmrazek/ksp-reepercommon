using System.Linq;
using Ploeh.AutoFixture.Xunit;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class SerializableFieldQueryTests
    {
        [Theory, AutoDomainData]
        public void Get_WithSimpleObject_ReturnsTheSingleSerializableField([Frozen] SimplePersistentObject targetObject, GetSerializableField sut)
        {
            var actual = sut.Get(targetObject).ToList();

            Assert.NotEmpty(actual);
            Assert.Single(actual); // simple object should only have one persistent field
        }


        [Theory, AutoDomainData]
        public void Get_WithComplexObject_ReturnsAllSerializableFields([Frozen] ComplexPersistentObject targetObject,
            GetSerializableField sut)
        {
            var actual = sut.Get(targetObject).ToList();

            Assert.NotEmpty(actual);
            Assert.True(actual.Count == 6); 
        }
    }
}
