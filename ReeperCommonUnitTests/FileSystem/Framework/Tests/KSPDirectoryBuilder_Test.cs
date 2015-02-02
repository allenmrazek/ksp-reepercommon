using ReeperCommon.FileSystem.Implementations;
using ReeperCommonUnitTests.FileSystem.Framework.Implementations;
using Xunit;

namespace ReeperCommonUnitTests.FileSystem.Framework.Tests
{
    public class KSPDirectoryBuilder_Test
    {
        static class KSPDirectoryBuilderFactory
        {
            public static FakeDirectoryBuilder Create(string name = "GameData")
            {
                return new FakeDirectoryBuilder(new FakeDirectory(name, new UrlFileMocker()), new FakeDirectoryFactory(new UrlFileMocker()));
            }
        }



        [Fact]
        private void Constructor_InvalidThrows()
        {
            //Assert.Throws<ArgumentNullException>(() => new FakeDirectoryBuilder(null, Substitute.For<IUrlFileMocker>()));
            //Assert.Throws<ArgumentNullException>(() => new FakeDirectoryBuilder("", Substitute.For<IUrlFileMocker>()));

            //Assert.Throws<ArgumentNullException>(
            //    () => new FakeDirectoryBuilder(null, Substitute.For<IFakeDirectoryBuilder>(), Substitute.For<IUrlFileMocker>()));
            //Assert.Throws<ArgumentNullException>(
            //    () => new FakeDirectoryBuilder("anonymous", (IUrlFileMocker)null));
        }


   



        [Fact]
        private void WithDirectory_Check()
        {
            var sut = KSPDirectoryBuilderFactory.Create();

            sut.WithDirectory("subdir");

            var result = sut.Build();

            Assert.NotNull(result);
            Assert.True(result.DirectoryExists(new KSPUrlIdentifier("subdir")));
            Assert.False(result.DirectoryExists(new KSPUrlIdentifier("fakeDir")));
            Assert.Empty(result.Files());
            Assert.Empty(result.RecursiveFiles());
        }



        //[Fact]
        //private void Build_OperatesOnLastItem()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    var second = sut.MakeDirectory("subdir");

        //    Assert.NotSame(second, sut);
        //}



        //[Fact]
        //private void BuildAll_OperatesOnTopmostBuilder()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    var result = sut.MakeDirectory("subdir")
        //                        .MakeDirectory("subsubdir")
        //                        .BuildAll(); // should result in equivalent of sut.Build

        //    var expected = sut.Build();

        //    Assert.NotEmpty(expected.RecursiveDirectories());
        //    Assert.NotEmpty(result.RecursiveDirectories());

        //    Assert.Equal(expected.RecursiveDirectories().Select(d => d.Name),
        //        result.RecursiveDirectories().Select(d => d.Name));

        //}



        //[Fact]
        //private void WithDirectory_AddsChild()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    sut.WithDirectory("Test");

        //    var result = sut.Build();

        //    Assert.NotEmpty(result.Directories());
        //    Assert.Equal("Test", result.Directories().Single().Name);

        //    Assert.NotEmpty(result.Directories());
        //    Assert.NotEmpty(result.UrlDir.Children);
        //    Assert.Equal("Test", result.UrlDir.Children.Single().Name);
        //}



        //[Fact]
        //private void WithDirectory_MultipleChildrenAndSubdirs()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    sut.WithDirectory("Test")
        //        .WithDirectory("AnotherTest")
        //        .WithFile("testfile.txt")
        //        .WithFile("testfile2.txt")
        //        .MakeDirectory("ThirdTest") // three subdirs total
        //            .WithFile("fileInThirdTestSubdir.txt")
        //            .WithDirectory("dirInThirdTestSubdir");
                    

        //    var result = sut.Build();

        //    Assert.NotEmpty(result.Directories());
        //    Assert.Equal(new[] { "Test", "AnotherTest", "ThirdTest" }, result.Directories().Select(d => d.Name));
        //    Assert.Equal(new[] {"Test", "AnotherTest", "ThirdTest", "dirInThirdTestSubdir" }, result.RecursiveDirectories().Select(d => d.Name));
        //}




        //[Fact]
        //private void WithDirectory_ThrowsInvalidOperation_OnDuplicateEntry()
        //{
        //    Assert.Throws<InvalidOperationException>(
        //        () => KSPDirectoryBuilderFactory.Create()
        //            .WithDirectory("test")
        //            .WithDirectory("test"));
        //}



        //[Fact]
        //private void WithFile_AddsFiles()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create()
        //        .WithFile("test.txt")
        //        .WithFile("second.other")
        //        .WithDirectory("subdir");

        //    var result = sut.Build();

        //    Assert.NotEmpty(result.Files());
        //    Assert.NotEmpty(result.RecursiveFiles());
        //    Assert.Equal(2, result.Files().Count());
        //    Assert.Equal(2, result.RecursiveFiles().Count());
        //    Assert.Equal(1, result.Files("txt").Count());
        //}






        //[Fact]
        //private void MakeDirectory_Check()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    sut.MakeDirectory("subdir").WithDirectory("test");

        //    var result = sut.Build();

        //    Assert.True(result.DirectoryExists(new KSPUrlIdentifier("subdir")));
        //    Assert.True(result.DirectoryExists(new KSPUrlIdentifier("subdir/test")));
        //    Assert.False(result.DirectoryExists(new KSPUrlIdentifier("fakeDir")));
        //    Assert.False(result.DirectoryExists(new KSPUrlIdentifier("subdir/fakedir")));
        //    Assert.Empty(result.Files());
        //    Assert.Empty(result.RecursiveFiles());
        //}



        //[Fact]
        //private void MakeDirectory_ThenWithDirectory_AddsDirAsSubDir()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();
        //    var subBuilder = sut.MakeDirectory("subdir");

        //    var resultNoDirs = subBuilder.Build();
        //    var sutResultNoDirs = sut.Build();

        //    var resultOneDir = subBuilder.WithDirectory("test").Build();
        //    var buildAllResult = subBuilder.BuildAll();
        //    var buildResult = sut.Build();

        //    Assert.Empty(resultNoDirs.Directories());
        //    Assert.NotEmpty(resultOneDir.Directories());
        //    Assert.Equal(1, resultOneDir.Directories().Count());
        //    Assert.Equal(1, buildAllResult.Directories().Count());
        //    Assert.Equal(1, buildResult.Directories().Count());

        //    Assert.Empty(resultNoDirs.Directories());
        //    Assert.Empty(sutResultNoDirs.FakeDirectory(new KSPUrlIdentifier("subdir")).Single().Directories());
          
        //    Assert.NotEmpty(resultOneDir.Directories());
        //    Assert.NotEmpty(buildAllResult.FakeDirectory(new KSPUrlIdentifier("subdir")).Single().Directories());
        //}



        //[Fact]
        //private void Parent_ThrowsInvalidOperation_WhenNoParentDir()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    Assert.Throws<InvalidOperationException>(() => sut.Parent());
        //}



        //[Fact]
        //private void Parent_ReturnsValid_WhenUsedByResultOfMakeDirectory()
        //{
        //    var sut = KSPDirectoryBuilderFactory.Create();

        //    var result = sut.MakeDirectory("any");

        //    Assert.NotNull(result.Parent());
        //}
    }
}
