using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSubstitute;
using NSubstitute.Core;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.Fixtures;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests
{
    public class SurrogateProviderTests
    {
        [Fact()]
        public void SurrogateProvider_ConstructorNullParams_Test()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    new SurrogateProvider(null, Substitute.For<IGetSurrogateSupportedTypes>(),
                        Substitute.For<IEnumerable<Assembly>>()));

            Assert.Throws<ArgumentNullException>(
                () =>
                    new SurrogateProvider(Substitute.For<IGetSerializationSurrogates>(), null,
                        Substitute.For<IEnumerable<Assembly>>()));

            Assert.Throws<ArgumentNullException>(
                () =>
                    new SurrogateProvider(Substitute.For<IGetSerializationSurrogates>(),
                        Substitute.For<IGetSurrogateSupportedTypes>(), null));

        }



    }

    public abstract class SurrogateProviderTypeTest<T>
    {
        [Fact]
        public void Get_WithGivenType_ContainsAResult_Test()
        {
            var sut = new SurrogateProvider(new GetSerializationSurrogates(new GetSurrogateSupportedTypes()),
                new GetSurrogateSupportedTypes(), new[] {typeof (IConfigNodeSerializer).Assembly});

            var result = sut.Get(typeof (T));

            Assert.NotEmpty(result);
        }
    }




    public class SurogateProviderStringTypeTest : SurrogateProviderTypeTest<string>
    {
    }

    public class SurogateProviderFloatTypeTest : SurrogateProviderTypeTest<float>
    {
    }

    public class SurogateProviderConfigNodeTypeTest : SurrogateProviderTypeTest<ConfigNode>
    {
    }
}
