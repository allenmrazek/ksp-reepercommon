using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class PersistenceMethodCallerTests
    {
        [Fact]
        public void PersistenceMethodCaller_ConstructorThrowsOnNullParameter_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new PersistenceMethodCaller(null));
        }


        [Theory, AutoDomainData]
        public void Serialize_WithIPersistenceSave_MethodCalled_Test(PersistenceMethodCaller sut, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var persistentSaveable = Substitute.For<IPersistenceSave>();
            var testObject = (object) persistentSaveable;

            sut.Serialize(testObject.GetType(), ref testObject, config, serializer);

            persistentSaveable.Received(1).PersistenceSave();
        }

        [Theory, AutoDomainData]
        public void Deserialize_WithIPersistenceLoad_MethodCalled_Test(PersistenceMethodCaller sut, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var persistentLoadable = Substitute.For<IPersistenceLoad>();
            var testObject = (object)persistentLoadable;

            sut.Deserialize(testObject.GetType(), ref testObject, config, serializer);

            persistentLoadable.Received(1).PersistenceLoad();
        }
    }
}
