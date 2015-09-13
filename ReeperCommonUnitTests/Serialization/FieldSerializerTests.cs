using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using NSubstitute;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class FieldSerializerTests
    {
        public class TestObject
        {
            [ReeperPersistent] public float FloatField;
            [ReeperPersistent] public string StringField;
        }


        [Fact]
        public void FieldSerializer_Constructor_ThrowsOnNullParameters_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new FieldSerializer(null, Substitute.For<IGetObjectFields>()));
            Assert.Throws<ArgumentNullException>(
                () => new FieldSerializer(Substitute.For<IConfigNodeItemSerializer>(), null));
            Assert.Throws<ArgumentNullException>(() => new FieldSerializer(null, null));
        }


        [Theory, AutoDomainData]
        public void Serialize_UsesSerializerForEachPersistentField_Test(
            IConfigNodeItemSerializer decoratedSerializer, 
            TestObject testObject, 
            IConfigNodeSerializer serializer,
            ConfigNode config)
        {
            var obj = (object)testObject;
            var fields = typeof (TestObject).GetFields(BindingFlags.Instance | BindingFlags.Public);
            var fieldQuery = Substitute.For<IGetObjectFields>();
            fieldQuery.Get(Arg.Is(testObject)).Returns(fields);

            serializer.SerializerSelector.GetSerializer(Arg.Any<Type>())
                .Returns(Substitute.For<IConfigNodeItemSerializer>().ToMaybe());

            var sut = new FieldSerializer(decoratedSerializer, fieldQuery);

            sut.Serialize(typeof (TestObject), ref obj, config, serializer);

            decoratedSerializer.Received()
                .Serialize(Arg.Is(typeof (TestObject)), ref obj, Arg.Any<ConfigNode>(), Arg.Is(serializer));
            serializer.SerializerSelector.Received().GetSerializer(Arg.Is(typeof (float)));
            serializer.SerializerSelector.Received().GetSerializer(Arg.Is(typeof (string)));
        }


        [Theory, AutoDomainData]
        public void Deserialize_UsesSerializerForEachPersistentField_Test(
            IConfigNodeItemSerializer decoratedSerializer,
            TestObject testObject,
            IConfigNodeSerializer serializer,
            ConfigNode config)
        {
            var obj = (object)testObject;
            var fields = typeof(TestObject).GetFields(BindingFlags.Instance | BindingFlags.Public);
            var fieldQuery = Substitute.For<IGetObjectFields>();
            fieldQuery.Get(Arg.Is(testObject)).Returns(fields);

            serializer.SerializerSelector.GetSerializer(Arg.Any<Type>())
                .Returns(Substitute.For<IConfigNodeItemSerializer>().ToMaybe());

            var sut = new FieldSerializer(decoratedSerializer, fieldQuery);

            sut.Deserialize(typeof(TestObject), ref obj, config, serializer);

            decoratedSerializer.Received()
                .Deserialize(Arg.Is(typeof(TestObject)), ref obj, Arg.Any<ConfigNode>(), Arg.Is(serializer));
            serializer.SerializerSelector.Received().GetSerializer(Arg.Is(typeof(float)));
            serializer.SerializerSelector.Received().GetSerializer(Arg.Is(typeof(string)));
        }
    }
}
