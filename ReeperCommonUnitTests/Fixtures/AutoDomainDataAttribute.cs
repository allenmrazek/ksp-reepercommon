using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Serialization;
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
            Fixture.Register(() => new GetSerializableField());
            Fixture.Register(() => new DefaultSurrogateProvider());
            Fixture.Register(() => new Rect(0f, 0f, 100f, 100f));
            Fixture.Register(
                () =>
                    new ConfigNodeSerializer(
                        new DefaultSerializerSelector(new SurrogateProvider(new[] {Assembly.GetExecutingAssembly()})),
                        new GetSerializableField()));

            Fixture.Register(() => new DefaultSerializerSelector());
        }
    }
    
}
