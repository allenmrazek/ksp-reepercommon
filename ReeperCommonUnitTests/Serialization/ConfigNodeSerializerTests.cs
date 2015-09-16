﻿using System;
using System.Linq;
using NSubstitute;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.Serialization.Complex;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public abstract class ConfigNodeSerializerTests<T>
    {
        [Fact]
        public void ConfigNodeSerializer_ConstructorThrowsOnNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigNodeSerializer(null));
        }


        [Theory, AutoDomainData]
        public void CreateConfigNodeFromObject_SearchesForSerializerOfSameType_UsesItToSerialize_Test(T data)
        {
            var selector = Substitute.For<ISerializerSelector>();
            var surrogate = Substitute.For<IConfigNodeItemSerializer>();

            selector.GetSerializer(Arg.Any<Type>()).Returns(ci => surrogate.ToMaybe());

            var sut = new ConfigNodeSerializer(selector);
            var dataObj = (object) data;

            sut.CreateConfigNodeFromObject(dataObj);

            selector.Received().GetSerializer(Arg.Is(typeof(T)));
            surrogate.Received().Serialize(Arg.Is(typeof(T)), ref dataObj, Arg.Any<string>(), Arg.Any<ConfigNode>(), Arg.Is(sut));
        }



        [Theory, AutoDomainData]
        public void WriteObjectToConfigNode_UsesSerializerForType_Test(T data)
        {
            var selector = Substitute.For<ISerializerSelector>();
            var surrogate = Substitute.For<IConfigNodeItemSerializer>();

            selector.GetSerializer(Arg.Any<Type>()).Returns(ci => surrogate.ToMaybe());

            var sut = new ConfigNodeSerializer(selector);
            var dataObj = (object)data;

            sut.WriteObjectToConfigNode(ref dataObj, new ConfigNode());

            selector.Received().GetSerializer(Arg.Is(typeof(T)));
            surrogate.Received().Serialize(Arg.Is(typeof(T)), ref dataObj, Arg.Any<string>(), Arg.Any<ConfigNode>(), Arg.Is(sut));
        }



        [Theory, AutoDomainData]
        public void LoadObjectFromConfigNode_UsesSerializerForType_Test(T data, ConfigNode config)
        {
            var selector = Substitute.For<ISerializerSelector>();
            var surrogate = Substitute.For<IConfigNodeItemSerializer>();

            selector.GetSerializer(Arg.Any<Type>()).Returns(ci => surrogate.ToMaybe());

            var sut = new ConfigNodeSerializer(selector);
            var dataObj = (object)data;

            sut.LoadObjectFromConfigNode(ref dataObj, new ConfigNode());

            selector.Received().GetSerializer(Arg.Is(typeof(T)));
            surrogate.Received().Deserialize(Arg.Is(typeof(T)), ref dataObj, Arg.Any<string>(), Arg.Any<ConfigNode>(), Arg.Is(sut));
        }
    }


    public class ConfigNodeSerializerTestsWithString : ConfigNodeSerializerTests<string>
    {
        
    }

    public class ConfigNodeSerializerTestsWithFloat : ConfigNodeSerializerTests<float>
    {
        
    }

    public class ConfigNodeSerializerTestsWithSimple : ConfigNodeSerializerTests<SimplePersistentObject>
    {
        
    }


    public class LiveTests
    {
        [Theory, AutoDomainData]
        public void CreateConfigNodeFromObject_WithNativeObject_ThatHasNativeField_DoesNotResultInTwoNativeDataNodesCoexisting_Test()
        {
            var testObject = new NativeSerializableObjectWithNativeSerializableField();
            var serializer =
                new DefaultConfigNodeSerializer(
                    AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => a.GetName().Name.StartsWith("ReeperCommon"))
                        .ToArray());

            var result = serializer.CreateConfigNodeFromObject(testObject);

            Assert.True(result.HasData);
            Assert.Equal(1, result.GetNodes(NativeSerializer.NativeNodeName).Length);
        }
    }
}
