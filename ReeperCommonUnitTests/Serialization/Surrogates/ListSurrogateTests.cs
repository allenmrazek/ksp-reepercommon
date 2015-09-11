using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSubstitute;
using Ploeh.AutoFixture;
using ReeperCommon.Containers;
using ReeperCommon.Serialization.Surrogates;
using ReeperCommonUnitTests.Fixtures;
using ReeperCommonUnitTests.TestData;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Surrogates.Tests
{
// ReSharper disable once ClassNeverInstantiated.Global


    public abstract class ListSurrogateTests<T>
    {


        class Factory
        {
            public static IFixture Create()
            {
                var fixture = new Fixture {RepeatCount = 5};

                fixture.Customize(new DomainCustomization());

                fixture.Register(() => new NativeSerializableType());
                fixture.Register(() => new ConfigNode());

                var itemSurrogate = Substitute.For<IConfigNodeItemSerializer>();
                var serializer = Substitute.For<IConfigNodeSerializer>();
                serializer.ConfigNodeItemSerializerSelector.GetSerializer(Arg.Any<Type>())
                    .Returns(ci => itemSurrogate.ToMaybe());

                fixture.Register(() => itemSurrogate);
                fixture.Register(() => serializer);
                fixture.Register(() => new SimplePersistentObjectWithList());


                return fixture;
            }
        }

        [Fact]
        public void SerializeTest()
        {
            var fixture = Factory.Create();


            
            var testList = new List<T>(fixture.CreateMany<T>());
            var config = fixture.CreateAnonymous<ConfigNode>();
            var itemSurrogate = fixture.CreateAnonymous<IConfigNodeItemSerializer>();

            var sut = new ListSurrogate<T>();

            sut.Serialize(typeof (List<T>), testList, fixture.CreateAnonymous<string>(), config, fixture.CreateAnonymous<IConfigNodeSerializer>());

            itemSurrogate.Received(fixture.RepeatCount)
                .Serialize(Arg.Is(typeof(T)), Arg.Any<object>(), Arg.Any<string>(), Arg.Any<ConfigNode>(),
                    Arg.Any<IConfigNodeSerializer>());

        }

        [Fact]
        public void Serialize_WithActualListField_Test()
        {
            var fixture = Factory.Create();
            var testObject = new SimplePersistentObjectWithList();
            var config = fixture.CreateAnonymous<ConfigNode>();

            var selector = new DefaultConfigNodeItemSerializerSelector(new DefaultSurrogateProvider(new GetSerializationSurrogates(new GetSurrogateSupportedTypes()), new GetSurrogateSupportedTypes()));
            //selector.AddSerializer(typeof (List<>),
            //    target => ((IConfigNodeItemSerializer)Activator.CreateInstance(
            //        typeof (ListSurrogate<>)
            //        .MakeGenericType(new[] {target.GetGenericArguments()[0]})))
            //        .ToMaybe());

            

            var serializer = new ConfigNodeSerializer(selector, new GetSerializableFields());
            serializer.Serialize(testObject, config);


            Assert.True(config.HasData);
            Assert.Equal(1, config.CountNodes);
            Assert.Equal(testObject.ListField.Count,
                typeof (SimplePersistentObjectWithList).GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(
                        pi =>
                            pi.FieldType.IsGenericType &&
                            pi.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    .With(pi => pi.Name)
                    .With(config.GetNode)
                    .Return(c => c.CountNodes, 0));
        }

        [Fact()]
        public void DeserializeTest()
        {
            Assert.True(false, "not implemented yet");
        }
    }


    public class ListSurrogateStringTests : ListSurrogateTests<string>
    {
        
    }

    public class ListSurrogateIntTests : ListSurrogateTests<int>
    {

    }

    //public class ListSurrogateWithNativeSerializableTypeTests : ListSurrogateTests<NativeSerializableType>
    //{

    //}
}
