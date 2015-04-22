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
        public void GetTest_SimpleObject_WithOneSerializedField([Frozen] SimplePersistentObject targetObject, SerializableFieldQuery sut)
        {
            var results = sut.Get(targetObject).ToList();

            Assert.NotEmpty(results);
            Assert.Single(results); // simple object should only have one persistent field
        }


        [Theory, AutoDomainData]
        public void GetTest_ComplexObject_WithTwoSerializableFields([Frozen] ComplexPersistentObject targetObject,
            SerializableFieldQuery sut)
        {
            var results = sut.Get(targetObject).ToList();

            Assert.NotEmpty(results);
            Assert.True(results.Count == 6); 
        }
    }
}
