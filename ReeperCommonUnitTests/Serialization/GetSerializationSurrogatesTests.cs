using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Serialization;
using Xunit;

namespace ReeperCommonUnitTests.Serialization
{
    public class GetSerializationSurrogatesTests
    {
        [Fact()]
        public void Get_WithNullArg_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new GetSerializationSurrogates(null));
        }


        [Fact()]
        public void Get_WithReeperCommonAssembly_LocatesSerializationSurrogates()
        {
            var sut = new GetSerializationSurrogates(new GetSurrogateSupportedTypes());

            var results = sut.Get(typeof(IGetSerializationSurrogates).Assembly).ToList();

            Assert.NotEmpty(results);
        }
    }
}
