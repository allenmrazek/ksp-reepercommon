using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace ReeperCommonUnitTests.Fixtures
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization()
            : base(new MultipleCustomization(), new AutoNSubstituteCustomization())
        {
        }
    }
}
