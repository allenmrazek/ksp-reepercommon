//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using NSubstitute;
//using Ploeh.AutoFixture;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.Serialization;
//using ReeperCommon.Serialization.Surrogates;
//using ReeperCommonUnitTests.Fixtures;
//using ReeperCommonUnitTests.TestData;
//using UnityEngine;
//using Xunit;
//using Xunit.Extensions;
//using Random = System.Random;

//// ReSharper disable once CheckNamespace
//namespace ReeperCommonUnitTests.Serialization.Surrogates
//{
//// ReSharper disable once ClassNeverInstantiated.Global


//    public abstract class ListSurrogateTests<T>
//    {
//        static class Factory
//        {
//// ReSharper disable once StaticFieldInGenericType
//            private static readonly Random RandomGenerator = new Random();

//            public static IFixture Create()
//            {
//                var fixture = new Fixture {RepeatCount = 5};
//                var rnd = new Random();

//                fixture.Customize(new DomainCustomization());

//                fixture.Register(() => new NativeSerializableType());
//                fixture.Register(() => new ConfigNode());

//                var itemSurrogate = Substitute.For<IConfigNodeItemSerializer>();
//                var serializer = Substitute.For<IConfigNodeSerializer>();
//                serializer.ConfigNodeItemSerializerSelector.GetSerializer(Arg.Any<Type>())
//                    .Returns(ci => itemSurrogate.ToMaybe());

//                fixture.Register(() => itemSurrogate);
//                fixture.Register(() => serializer);
//                //fixture.Register(() => new SimplePersistentObjectWithList());
//                fixture.Register(() => new Rect(NextFloat(), NextFloat(), NextFloat(), NextFloat()));
//                //fixture.Register(() => new ComplexPersistentObject());
//                fixture.Register(() => new NativeSerializableType {FloatField = NextFloat()});
//                fixture.Register(() => new SimplePersistentObjectWithList {ListField = fixture.CreateMany<T>().ToList() });

//                return fixture;
//            }


//            private static float NextFloat()
//            {
//                return (float)(RandomGenerator.NextDouble());
//            }
//        }


//        private class SimplePersistentObjectWithList
//        {
//            [ReeperPersistent, Persistent]
//            public List<T> ListField = new List<T>
//            {
//                default(T), default(T), default(T)
//            };
//        }


//        [Fact]
//        public void SerializeTest()
//        {
//            var fixture = Factory.Create();

//            var testList = new List<T>(fixture.CreateMany<T>());
//            var config = fixture.CreateAnonymous<ConfigNode>();

//            var itemSurrogate = Substitute.For<IConfigNodeItemSerializer>();
//            var serializer = Substitute.For<IConfigNodeSerializer>();
//            serializer.ConfigNodeItemSerializerSelector.GetSerializer(Arg.Any<Type>())
//                .Returns(ci => itemSurrogate.ToMaybe());
            
//            var sut = new ListSurrogate<T>();

//            sut.Serialize(typeof (List<T>), testList, fixture.CreateAnonymous<string>(), config, serializer);

//            itemSurrogate.Received(fixture.RepeatCount).Serialize(
//                Arg.Is(typeof(T)), Arg.Any<object>(), Arg.Any<string>(), Arg.Any<ConfigNode>(),
//                Arg.Is(serializer));
//        }


//        [Fact]
//        public void Serialize_WithActualListField_Test()
//        {
//            var fixture = Factory.Create();
//            var testObject = new SimplePersistentObjectWithList {ListField = fixture.CreateMany<T>().ToList()};
//            //var config = fixture.CreateAnonymous<ConfigNode>();

//            var selector = new DefaultConfigNodeItemSerializerSelector(new DefaultSurrogateProvider(new GetSerializationSurrogates(new GetSurrogateSupportedTypes()), new GetSurrogateSupportedTypes()));

//            var serializer = new ConfigNodeSerializer(selector, new GetSerializableFields());
//            var config = serializer.CreateConfigNodeFromObject(testObject); //serializer.Serialize(testObject, config));


//            Assert.True(config.HasData);
//            Assert.Equal(1, config.CountNodes);
//            Assert.Equal(testObject.ListField.Count,
//                typeof (SimplePersistentObjectWithList).GetFields(BindingFlags.Public | BindingFlags.Instance)
//                    .FirstOrDefault(
//                        pi =>
//                            pi.FieldType.IsGenericType &&
//                            pi.FieldType.GetGenericTypeDefinition() == typeof(List<>))
//                    .With(pi => pi.Name)
//                    .With(config.GetNode)
//                    .Return(c => c.CountNodes, 0));

//            if (typeof(T) != typeof(ComplexPersistentObject)) return;
//            config.Write("D:\\ListSurrogateTest.cfg", "Surrogate Test");
//            ConfigNode.CreateConfigFromObject(testObject).Write("D:\\Persistent.cfg", string.Empty);
//        }


//        //[Fact]
//        //public void DeserializeTest()
//        //{
//        //    var fixture = Factory.Create();

//        //    var testList = new List<T>(fixture.CreateMany<T>());
//        //    var config = fixture.CreateAnonymous<ConfigNode>();

//        //    var itemSurrogate = Substitute.For<IConfigNodeItemSerializer>();
//        //    var serializer = Substitute.For<IConfigNodeSerializer>();
//        //    serializer.ConfigNodeItemSerializerSelector.GetSerializer(Arg.Any<Type>())
//        //        .Returns(ci => itemSurrogate.ToMaybe());

//        //    itemSurrogate.Deserialize(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<string>(), Arg.Any<ConfigNode>(),
//        //        Arg.Is(serializer))
//        //        .Returns(ci => ci.Arg<object>());

//        //    var sut = new ListSurrogate<T>();
//        //    var key = fixture.CreateAnonymous<string>();

//        //    sut.Serialize(typeof(List<T>), testList, key, config, serializer);
//        //    sut.Deserialize(typeof(List<T>), testList, key, config, serializer);

//        //    itemSurrogate.Received(fixture.RepeatCount).Deserialize(
//        //        Arg.Is(typeof(T)), Arg.Any<object>(), Arg.Any<string>(), Arg.Any<ConfigNode>(),
//        //        Arg.Is(serializer));
//        //}



//        [Fact]
//        public void Deserialize_WithActualListField_Test()
//        {
//            var fixture = Factory.Create();
//            var originalContents = fixture.CreateMany<T>().ToList();
//            var testObject = new SimplePersistentObjectWithList { ListField = originalContents };
//            var config = fixture.CreateAnonymous<ConfigNode>();

//            var selector = new DefaultConfigNodeItemSerializerSelector(new DefaultSurrogateProvider(new GetSerializationSurrogates(new GetSurrogateSupportedTypes()), new GetSurrogateSupportedTypes()));

//            var serializer = new ConfigNodeSerializer(selector, new GetSerializableFields());
//            serializer.Serialize(testObject, config);

//            testObject.ListField = fixture.CreateMany<T>().Union(fixture.CreateMany<T>()).ToList(); // create more random stuff
//            serializer.Deserialize(testObject, config);

//            if (typeof (T) == typeof (NativeSerializableType))
//                config.Write("D:\\NativeSerializeTypeDebug.cfg", string.Empty);

//            Assert.True(config.HasData);
//            Assert.Equal(1, config.CountNodes);
//            Assert.Equal(testObject.ListField.Count,
//                typeof(SimplePersistentObjectWithList).GetFields(BindingFlags.Public | BindingFlags.Instance)
//                    .FirstOrDefault(
//                        pi =>
//                            pi.FieldType.IsGenericType &&
//                            pi.FieldType.GetGenericTypeDefinition() == typeof(List<>))
//                    .With(pi => pi.Name)
//                    .With(config.GetNode)
//                    .Return(c => c.CountNodes, 0));
//            Assert.NotEmpty(testObject.ListField);
//            Assert.Equal(fixture.RepeatCount, testObject.ListField.Count);
//            Assert.True(originalContents.All(originalValue => testObject.ListField.Contains(originalValue)));
//        }
//    }


//    public class ListSurrogateStringTests : ListSurrogateTests<string>
//    {
        
//    }

//    public class ListSurrogateIntTests : ListSurrogateTests<int>
//    {

//    }

//    public class ListSurrogateWithNativeSerializableTypeTests : ListSurrogateTests<NativeSerializableType>
//    {

//    }

//    public class ListSurrogateWithSurrogateSerializableTypeTests : ListSurrogateTests<ComplexPersistentObject>
//    {

//    }
//}
