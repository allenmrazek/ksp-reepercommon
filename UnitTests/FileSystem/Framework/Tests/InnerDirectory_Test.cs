using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using UnitTests.FileSystem.Framework.Implementations;
using Xunit;

namespace UnitTests.FileSystem.Framework.Tests
{
    public class InnerDirectory_Test
    {
        static class InnerDirectoryFactory
        {
            public static InnerDirectory Create(string name, IDirectoryBuilder builder, IUrlFileMocker fmocker)
            {
                return new InnerDirectory(name, builder, fmocker);
            }

            public static InnerDirectory Create(string name, IDirectoryBuilder builder)
            {
                return Create(name, builder, new UrlFileMocker());
            }
        }



        [Fact]
        void Constructor_ThrowsArgumentNullException_OnNullOrInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => new InnerDirectory(null, Substitute.For<IDirectoryBuilder>(), Substitute.For<IUrlFileMocker>()));
            Assert.Throws<ArgumentNullException>(() => new InnerDirectory("", Substitute.For<IDirectoryBuilder>(), Substitute.For<IUrlFileMocker>()));

            Assert.Throws<ArgumentNullException>(() => new InnerDirectory("anonymous", null, Substitute.For<IUrlFileMocker>()));
            Assert.Throws<ArgumentNullException>(
                () => new InnerDirectory("anonymous", Substitute.For<IDirectoryBuilder>(), null));

        }



        [Fact]
        void Build_CallsAssignedBuilder()
        {
            // arrange
            var builder = Substitute.For<IDirectoryBuilder>();

            var sut = InnerDirectoryFactory.Create("Test", builder);

            
            // act
            sut.Build();


            // assert
            builder.Build().Received();
        }
    }
}
