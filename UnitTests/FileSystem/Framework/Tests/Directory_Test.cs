using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using UnitTests.FileSystem.Framework.Implementations;
using Xunit;

namespace UnitTests.FileSystem.Framework.Tests
{
    public class Directory_Test
    {
        static class DirectoryFactory
        {
            public static IDirectory Create(string name, IUrlFileMocker fmocker)
            {
                return new Directory(name, fmocker);
            }

            public static IDirectory Create(string name)
            {
                return new Directory(name, new UrlFileMocker());
            }
        }



        [Fact]
        void Constructor_ThrowsExceptionOnNull_OrEmpty_OrBad_String()
        {
            Assert.Throws<ArgumentNullException>(() => new Directory(null, Substitute.For<IUrlFileMocker>()));
            Assert.Throws<ArgumentNullException>(() => new Directory("", Substitute.For<IUrlFileMocker>()));
            Assert.Throws<ArgumentNullException>(() => new Directory("anonymous", null));
            Assert.Throws<ArgumentNullException>(() => new Directory("/", Substitute.For<IUrlFileMocker>()));
            Assert.Throws<ArgumentNullException>(() => new Directory("\\", Substitute.For<IUrlFileMocker>()));
        }



        [Fact]
        void Build_CallsAssignedBuilder()
        {
            // arrange
            var builder = Substitute.For<IDirectoryBuilder>();

            var sut = DirectoryFactory.Create("Test");


            // act
            sut.Build();


            // assert
            builder.Build().Received();
        }


    }
}
