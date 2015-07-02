using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.TestData;

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
        }
    }
    
}
