using System;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.TestData;
using UnityEngine;
using Random = System.Random;

namespace ReeperCommonUnitTests.Fixtures
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
            var rnd = new Random();

            Fixture.Register(() => new ConfigNode("root"));
            Fixture.Register(() => new SimplePersistentObject());
            Fixture.Register(() => new ComplexPersistentObject());
            Fixture.Register(() => new GetSerializableFields());
            //Fixture.Register(() => new DefaultSurrogateProvider());
            Fixture.Register(() => new Rect(0f, 0f, 100f, 100f));
            //Fixture.Register(
            //    () =>
            //        new ConfigNodeSerializer(
            //            new DefaultConfigNodeItemSerializerSelector(new SurrogateProvider(new[] {Assembly.GetExecutingAssembly(), typeof(ConfigNodeSerializer).Assembly})),
            //            new GetSerializableFields()));

            //Fixture.Register(() => new DefaultConfigNodeItemSerializerSelector());
            Fixture.Register(() => new GetSurrogateSupportedTypes());
            //Fixture.Register(() => new PrimitiveSurrogateSerializer());
            Fixture.Register(() => new NativeSerializer());
            //Fixture.Register(() => new PersistenceMethodCaller(Substitute.For<IConfigNodeItemSerializer>()));

            Fixture.Register(() =>
            {
                var serializer = new ConfigNodeSerializer(
                    new SerializerSelectorDecorator(
                        new PreferNativeSerializer(
                            new SerializerSelector(
                                new SurrogateProvider(
                                    new GetSerializationSurrogates(new GetSurrogateSupportedTypes()),
                                    new GetSurrogateSupportedTypes(),
                                    AppDomain.CurrentDomain.GetAssemblies()
                                        .Where(a => a.GetName().Name.StartsWith("ReeperCommon")).ToArray()))),
                    result => Maybe<IConfigNodeItemSerializer>.With(new FieldSerializer(result, new GetSerializableFields()))));

                return serializer;
            });

            Fixture.Register(
                () => new Quaternion((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
        }
    }
    
}
