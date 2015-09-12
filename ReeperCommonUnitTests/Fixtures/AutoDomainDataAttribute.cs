using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Surrogates;
using ReeperCommonUnitTests.TestData;
using UnityEngine;

namespace ReeperCommonUnitTests.Fixtures
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
            Fixture.Register(() => new ConfigNode("ROOT"));
            Fixture.Register(() => new SimplePersistentObject());
            Fixture.Register(() => new ComplexPersistentObject());
            Fixture.Register(() => new GetSerializableFields());
            Fixture.Register(() => new DefaultSurrogateProvider());
            Fixture.Register(() => new Rect(0f, 0f, 100f, 100f));
            Fixture.Register(
                () =>
                    new ConfigNodeSerializer(
                        new DefaultConfigNodeItemSerializerSelector(new SurrogateProvider(new[] {Assembly.GetExecutingAssembly(), typeof(ConfigNodeSerializer).Assembly})),
                        new GetSerializableFields()));

            Fixture.Register(() => new DefaultConfigNodeItemSerializerSelector());
            Fixture.Register(() => new GetSurrogateSupportedTypes());
            Fixture.Register(() => new PrimitiveSurrogateSerializer());
            Fixture.Register(() => new NativeSerializer());
            Fixture.Register(() => new ReeperPersistentMethodCaller(Substitute.For<IConfigNodeItemSerializer>()));
        }
    }
    
}
