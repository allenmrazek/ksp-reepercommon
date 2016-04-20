using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using UnityEngine;
using Random = System.Random;

namespace ReeperCommonUnitTests.Fixtures
{
    internal class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
            var rnd = new Random();

            Fixture.Register(() => new ConfigNode("root"));
            Fixture.Register(() => new Rect(0f, 0f, 100f, 100f));
            Fixture.Register(
                () => new Quaternion((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
        }
    }
    
}
