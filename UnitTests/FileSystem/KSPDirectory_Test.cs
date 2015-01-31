using System;
using System.Linq;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using NSubstitute;
using UnitTests.FileSystem.Framework;
using UnitTests.FileSystem.Framework.Implementations;
using Xunit;
using IDirectory = ReeperCommon.FileSystem.IDirectory;

namespace UnitTests.FileSystem
{
    public class KSPDirectory_Test
    {
        private static class Factory
        {
            public static IDirectory Create(string name, IUrlDir root)
            {
                return new KSPDirectory(CreateFileSystemFactory(), root);
            }

       
            public static IFileSystemFactory CreateFileSystemFactory()
            {
                var fsf = Substitute.For<IFileSystemFactory>();

                fsf.GetDirectory(Arg.Any<IUrlDir>()).ReturnsForAnyArgs(d => new KSPDirectory(fsf, d.Arg<IUrlDir>()));
                fsf.GetFile(Arg.Any<IDirectory>(), Arg.Any<IUrlFile>()).Returns(d => new KSPFile(d.Arg<IDirectory>(), d.Arg<IUrlFile>()));

                return fsf;
            }

            public static IDirectoryBuilder CreateBuilder()
            {
                return new KSPDirectoryBuilder("GameData", new UrlFileMocker());
            }
        }



        [Fact]
        void Constructor_NullArgument_Check()
        {
            Assert.Throws<ArgumentNullException>(() => new KSPDirectory(null, Substitute.For<IUrlDir>()));
            Assert.Throws<ArgumentNullException>(() => new KSPDirectory(Substitute.For<IFileSystemFactory>(), null));
        }



        [Fact]
        void Directories_ReturnsDirectSubdirectoriesOnly()
        {
            var builder = new KSPDirectoryBuilder("GameData", new UrlFileMocker());

            builder
                .WithFile("file.txt")
                .WithDirectory("TestDirectory")
                .WithDirectory("AnotherTest")
                .MakeDirectory("Subdir")
                    .WithDirectory("DirInSubdir")
                    .WithFile("subfile.txt");

            var sut = builder.Build();

            var directChildDirectories = sut.Directories();

            Assert.Equal(new[] { "TestDirectory", "AnotherTest", "Subdir" },
                directChildDirectories.Select(dir => dir.Name));

        }



        [Fact]
        void RecursiveDirectories_ReturnsAllDirectoriesUnder()
        {
            var builder = new KSPDirectoryBuilder("GameData", new UrlFileMocker());

            var sut = builder
                        .WithFile("file.txt")
                        .MakeDirectory("first")
                            .MakeDirectory("second")
                                .MakeDirectory("third")
                                .WithFile("first_second_third.txt")
                        .BuildAll();
            
            var allDirs = sut.RecursiveDirectories();

            Assert.NotEmpty(allDirs);
            Assert.Equal(new[] {"first", "second", "third"}, allDirs.Select(d => d.Name));
        }


        
        [Fact]
        void Directory()
        {
            var sut = Factory.CreateBuilder()
                        .WithDirectory("first")
                        .WithDirectory("second")
                        .MakeDirectory("third")
                            .WithDirectory("fourth")
                            .BuildAll();

            Assert.True(sut.Directory(new KSPUrlIdentifier("first")).Any());
            Assert.True(sut.Directory(new KSPUrlIdentifier("third/fourth")).Any());
            Assert.False(sut.Directory(new KSPUrlIdentifier("nonexistent")).Any());
            Assert.False(sut.Directory(new KSPUrlIdentifier("fake/fourth")).Any());
        }



        [Fact]
        void DirectoryExists()
        {
            var sut = Factory.CreateBuilder()
                .MakeDirectory("test")
                .WithDirectory("subdir").BuildAll();

            Assert.True(sut.DirectoryExists(new KSPUrlIdentifier("test")));
            Assert.True(sut.DirectoryExists(new KSPUrlIdentifier("test/subdir")));
            Assert.False(sut.DirectoryExists(new KSPUrlIdentifier("fake")));
            Assert.False(sut.DirectoryExists(new KSPUrlIdentifier("fake/subdir")));
        }



        [Fact]
        void File()
        {
            var sut = Factory.CreateBuilder()
                                .WithFile("test.txt")
                                .WithFile("another")
                                    .MakeDirectory("subdir")
                                    .WithFile("subfile.txt").BuildAll();

            Assert.True(sut.File(new KSPUrlIdentifier("test.txt")).Any());
            Assert.True(sut.File(new KSPUrlIdentifier("test")).Any()); // remember -- we accept extensionless also
            Assert.True(sut.File(new KSPUrlIdentifier("another")).Any());
            Assert.True(sut.File(new KSPUrlIdentifier("subdir/subfile.txt")).Any());
            Assert.True(sut.File(new KSPUrlIdentifier("subdir/subfile")).Any());

            Assert.False(sut.File(new KSPUrlIdentifier("nonexistent")).Any());
            Assert.False(sut.File(new KSPUrlIdentifier("subdir/nonexistent")).Any());
            Assert.False(sut.File(new KSPUrlIdentifier("nonexistent/subfile.txt")).Any());
        }



        [Fact]
        void File_FindsBestMatch()
        {
            var sut = Factory.CreateBuilder()
                .WithFile("test.txt")
                .WithFile("test.txt.txt")
                .WithFile("test")
                .Build();

            Assert.Equal("test", sut.File(new KSPUrlIdentifier("test")).Single().FileName);
            Assert.Equal("test.txt", sut.File(new KSPUrlIdentifier("test.txt")).Single().FileName);
            Assert.Equal("test.txt.txt", sut.File(new KSPUrlIdentifier("test.txt.txt")).Single().FileName);
        }



        [Fact]
        void FileExists()
        {
            var sut = Factory.CreateBuilder()
                                .WithFile("test.txt")
                                .WithFile("test")
                                .MakeDirectory("subdir")
                                    .WithFile("subfile.txt")
                                    .BuildAll();

            Assert.True(sut.FileExists(new KSPUrlIdentifier("test.txt")));
            Assert.True(sut.FileExists(new KSPUrlIdentifier("test")));
            Assert.True(sut.FileExists(new KSPUrlIdentifier("subdir/subfile.txt")));
            Assert.True(sut.FileExists(new KSPUrlIdentifier("subdir/subfile")));

            Assert.False(sut.FileExists(new KSPUrlIdentifier("nonexistent")));
            Assert.False(sut.FileExists(new KSPUrlIdentifier("subdir/nonexistent")));
            Assert.False(sut.FileExists(new KSPUrlIdentifier("nonexistent/subfile.txt")));
        }



        void Files()
        {
            
        }

        void RecursiveFiles()
        {
            
        }

        void FullPath_Property()
        {
            
        }

        void Parent_Property()
        {
            
        }

        void Url_Property()
        {
            
        }

        void UrlDir_Property()
        {
            
        }

        void Name_Property()
        {
            
        }
    }



    //public class KSPDirectory_Test
    //{
    //    private static class Factory
    //    {
    //        private static IUrlFile CreateFileNoSubdirectory()
    //        {
    //            var file = Substitute.For<IUrlFile>();

    //            file.Name.Returns("file");
    //            file.Extension.Returns("txt");
    //            file.FullPath.Returns("C:/GameData/file.txt");
    //            file.Url.Returns("/file");

    //            return file;
    //        }

    //        private static IUrlFile CreateFileInSubdirectory()
    //        {
    //            var file = Substitute.For<IUrlFile>();

    //            file.Name.Returns("subfile");
    //            file.Extension.Returns("txt");
    //            file.FullPath.Returns("C:/GameData/subdir/subfile.txt");
    //            file.Url.Returns("/subdir/subfile");

    //            return file;

    //        }

    //        private static IUrlFile CreateFileInSub_Subdirectory()
    //        {
    //            var file = Substitute.For<IUrlFile>();

    //            file.Name.Returns("subsubfile");
    //            file.Extension.Returns("sub");
    //            file.FullPath.Returns("C:/GameData/subdir/subsubdir/subsubfile.txt");
    //            file.Url.Returns("/subdir/subsubdir/subsubfile");

    //            return file;
    //        }

    //        private static IUrlDir CreateUrlDirGameData()
    //        {
    //            var gdDir = Substitute.For<IUrlDir>();

    //            var fileNoSubDir = CreateFileNoSubdirectory();
    //            var fileSubDir = CreateFileInSubdirectory();
    //            var fileSubSubDir = CreateFileInSub_Subdirectory();

    //            gdDir.Name.Returns("GameData");
    //            gdDir.Parent.Returns((IUrlDir) null);
    //            gdDir.FullPath.Returns("C:/GameData");
    //            gdDir.Files.Returns(new[] {fileNoSubDir});
    //            gdDir.AllFiles.Returns(new[] {fileNoSubDir, fileSubDir, fileSubSubDir});
    //            gdDir.Url.Returns("/");

    //            var subdir = Substitute.For<IUrlDir>();
    //            subdir.Name.Returns("subdir");
    //            subdir.Parent.Returns(gdDir);
    //            subdir.FullPath.Returns("C:/GameData/subdir");
    //            subdir.Files.Returns(new[] {fileSubDir});
    //            subdir.AllFiles.Returns(new[] {fileSubDir, fileSubSubDir});
    //            subdir.Url.Returns("/subdir");

    //            var subsubdir = Substitute.For<IUrlDir>();
    //            subsubdir.Name.Returns("subsubdir");
    //            subsubdir.Parent.Returns(subdir);
    //            subsubdir.FullPath.Returns("C:/GameData/subdir/subsubdir");
    //            subsubdir.Url.Returns("/subdir/subsubdir/");
    //            subsubdir.Files.Returns(new[] {fileSubSubDir});
    //            subsubdir.AllFiles.Returns(new[] {fileSubSubDir});

    //            gdDir.Children.Returns(new[] {subdir});
    //            subdir.Children.Returns(new[] {subsubdir});

    //            return gdDir;
    //        }

    //        private static IFileSystemFactory CreateFileSystemFactory()
    //        {
    //            var fsf = Substitute.For<IFileSystemFactory>();

    //            fsf.GetDirectory(Arg.Any<IUrlDir>()).ReturnsForAnyArgs(d => new KSPDirectory(fsf, d.Arg<IUrlDir>()));
    //            fsf.GetFile(Arg.Any<IDirectory>(), Arg.Any<IUrlFile>()).Returns(d => new KSPFile(d.Arg<IDirectory>(), d.Arg<IUrlFile>()));
 
    //            return fsf;
    //        }

    //        public static KSPDirectory Create()
    //        {
    //            return new KSPDirectory(CreateFileSystemFactory(), CreateUrlDirGameData());
    //        }
    //    }



    //    [Fact]
    //    private void Constructor_ArgumentNull_ThrowsException()
    //    {
    //        Assert.Throws<ArgumentNullException>(() => new KSPDirectory(null, Substitute.For<IUrlDir>()));
    //        Assert.Throws<ArgumentNullException>(() => new KSPDirectory(Substitute.For<IFileSystemFactory>(), null));
    //    }









    //    [Theory]
    //    [InlineData("file.txt")]
    //    [InlineData("/file.txt")]
    //    [InlineData("file")]
    //    [InlineData("/file")]
    //    [InlineData("\\file.txt")]
    //    [InlineData("\\file")]
    //    private void FileExists_InSameDir_ReturnsTrue_WithExistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }



    //    [Theory]
    //    [InlineData("subdir/subfile.txt")]
    //    [InlineData("/subdir/subfile.txt")]
    //    [InlineData("subdir/subfile")]
    //    [InlineData("/subdir/subfile")]
    //    [InlineData("\\subdir\\subfile.txt")]
    //    [InlineData("\\subdir\\subfile")]
    //    [InlineData("\\subdir/subfile.txt")]
    //    [InlineData("\\subdir/subfile")]
    //    [InlineData("/subdir\\subfile.txt")]
    //    [InlineData("/subdir\\subfile")]
    //    public void FileExists_InExistingSubDir_ReturnsTrue_WithExistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }



    //    [Theory]
    //    [InlineData("doesntExist.txt")]
    //    [InlineData("/doesntExist.txt")]
    //    [InlineData("doesntExist")]
    //    [InlineData("/doesntExist")]
    //    [InlineData("\\doesntExist.txt")]
    //    [InlineData("\\doesntExist")]
    //    private void FileExists_InSameDir_ReturnsFalse_WithNonExistentFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }



    //    [Theory]
    //    [InlineData("subdir/doesntExist.txt")]
    //    [InlineData("/subdir/doesntExist.txt")]
    //    [InlineData("subdir/doesntExist")]
    //    [InlineData("/subdir/doesntExist")]
    //    [InlineData("\\subdir\\doesntExist.txt")]
    //    [InlineData("\\subdir\\doesntExist")]
    //    [InlineData("\\subdir/doesntExist.txt")]
    //    [InlineData("\\subdir/doesntExist")]
    //    [InlineData("/subdir\\doesntExist.txt")]
    //    [InlineData("/subdir\\doesntExist")]
    //    private void FileExists_InExistingSubDir_ReturnsFalse_WithNonExistentFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }


    //    [Theory]
    //    [InlineData("badsubdir/doesntExist.txt")]
    //    [InlineData("/badsubdir/doesntExist.txt")]
    //    [InlineData("badsubdir/doesntExist")]
    //    [InlineData("/badsubdir/doesntExist")]
    //    [InlineData("\\badsubdir\\doesntExist.txt")]
    //    [InlineData("\\badsubdir\\doesntExist")]
    //    [InlineData("\\badsubdir/doesntExist.txt")]
    //    [InlineData("\\badsubdir/doesntExist")]
    //    [InlineData("/badsubdir\\doesntExist.txt")]
    //    [InlineData("/badsubdir\\doesntExist")]
    //    private void FileExists_InNonexistingSubDir_ReturnsFalse_WithNonExistentFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }



    //    [Theory]
    //    [InlineData("subdir/subsubdir/subsubfile.sub")]
    //    [InlineData("subdir/subsubdir/subsubfile")]
    //    [InlineData("subdir/subsubdir\\subsubfile.sub")]
    //    [InlineData("subdir/subsubdir\\subsubfile")]
    //    [InlineData("subdir\\subsubdir\\subsubfile.sub")]
    //    [InlineData("subdir\\subsubdir\\subsubfile")]
    //    [InlineData("subdir\\subsubdir/subsubfile.sub")]
    //    [InlineData("subdir\\subsubdir/subsubfile")]
    //    [InlineData("subdir/subsubdir\\subsubfile.sub")]
    //    [InlineData("subdir/subsubdir\\subsubfile")]
    //    [InlineData("/subdir/subsubdir/subsubfile.sub")]
    //    [InlineData("/subdir/subsubdir/subsubfile")]
    //    [InlineData("\\subdir\\subsubdir\\subsubfile.sub")]
    //    [InlineData("\\subdir\\subsubdir\\subsubfile")]
    //    private void FileExists_InSubSubDir_ReturnsTrue_WithExistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.FileExists(new KSPUrlIdentifier(filename)));
    //    }



    //    [Theory]
    //    [InlineData("file.txt")]
    //    [InlineData("/file.txt")]
    //    [InlineData("file")]
    //    [InlineData("/file")]
    //    [InlineData("\\file.txt")]
    //    [InlineData("\\file")]
    //    public void File_InSameDir_ReturnsFileWhichExists(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.File(new KSPUrlIdentifier(filename)).Any());
    //    }



    //    [Theory]
    //    [InlineData("doesntExist.txt")]
    //    [InlineData("/doesntExist.txt")]
    //    [InlineData("doesntExist")]
    //    [InlineData("/doesntExist")]
    //    [InlineData("\\doesntExist.txt")]
    //    [InlineData("\\doesntExist")]
    //    public void File_InSameDir_ReturnsNone_ForNonexistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.File(new KSPUrlIdentifier(filename)).Any());
    //    }


    //    [Theory]
    //    [InlineData("subdir/subfile.txt")]
    //    [InlineData("/subdir/subfile.txt")]
    //    [InlineData("subdir/subfile")]
    //    [InlineData("/subdir/subfile")]
    //    [InlineData("\\subdir\\subfile.txt")]
    //    [InlineData("\\subdir\\subfile")]
    //    [InlineData("\\subdir/subfile.txt")]
    //    [InlineData("\\subdir/subfile")]
    //    [InlineData("/subdir\\subfile.txt")]
    //    [InlineData("/subdir\\subfile")]
    //    public void File_InExistingSubDir_ReturnsFileWhichExists(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.File(new KSPUrlIdentifier(filename)).Any());
    //    }



    //    [Theory]
    //    [InlineData("subdir/doesntExist.txt")]
    //    [InlineData("/subdir/doesntExist.txt")]
    //    [InlineData("subdir/doesntExist")]
    //    [InlineData("/subdir/doesntExist")]
    //    [InlineData("\\subdir\\doesntExist.txt")]
    //    [InlineData("\\subdir\\doesntExist")]
    //    [InlineData("\\subdir/doesntExist.txt")]
    //    [InlineData("\\subdir/doesntExist")]
    //    [InlineData("/subdir\\doesntExist.txt")]
    //    [InlineData("/subdir\\doesntExist")]
    //    public void File_InExistingSubDir_ReturnsNone_ForNonexistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.File(new KSPUrlIdentifier(filename)).Any());
    //    }



    //    [Theory]
    //    [InlineData("badsubdir/doesntExist.txt")]
    //    [InlineData("/badsubdir/doesntExist.txt")]
    //    [InlineData("badsubdir/doesntExist")]
    //    [InlineData("/badsubdir/doesntExist")]
    //    [InlineData("\\badsubdir\\doesntExist.txt")]
    //    [InlineData("\\badsubdir\\doesntExist")]
    //    [InlineData("\\badsubdir/doesntExist.txt")]
    //    [InlineData("\\badsubdir/doesntExist")]
    //    [InlineData("/badsubdir\\doesntExist.txt")]
    //    [InlineData("/badsubdir\\doesntExist")]
    //    public void File_InNonexistingSubDir_ReturnsNone_ForNonexistingFile(string filename)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.File(new KSPUrlIdentifier(filename)).Any());
    //    }



    //    [Theory]
    //    [InlineData("subdir")]
    //    [InlineData("/subdir")]
    //    [InlineData("/subdir/")]
    //    [InlineData("\\subdir")]
    //    [InlineData("\\subdir\\")]
    //    [InlineData("\\subdir/")]
    //    [InlineData("/subdir\\")]
    //    public void DirectoryExists_ThatIsADirectSubDir_ReturnsTrue_ForExistingDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.DirectoryExists(new KSPUrlIdentifier(dirname)));
    //    }



    //    [Theory]
    //    [InlineData("subdir")]
    //    [InlineData("/subdir")]
    //    [InlineData("/subdir/")]
    //    [InlineData("\\subdir")]
    //    [InlineData("\\subdir\\")]
    //    [InlineData("\\subdir/")]
    //    [InlineData("/subdir\\")]
    //    public void Directory_ThatIsADirectSubDir_ReturnsValid_ForExistingDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.Directory(new KSPUrlIdentifier(dirname)).Any());
    //    }



    //    [Theory]
    //    [InlineData("nonexistent")]
    //    [InlineData("/nonexistent")]
    //    [InlineData("/nonexistent/")]
    //    [InlineData("\\nonexistent")]
    //    [InlineData("\\nonexistent\\")]
    //    [InlineData("\\nonexistent/")]
    //    [InlineData("/nonexistent\\")]
    //    public void DirectoyrExists_ThatIsADirectSubDir_ReturnsNone_ForNonexistingDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.DirectoryExists(new KSPUrlIdentifier(dirname)));
    //    }



    //    [Theory]
    //    [InlineData("nonexistent")]
    //    [InlineData("/nonexistent")]
    //    [InlineData("/nonexistent/")]
    //    [InlineData("\\nonexistent")]
    //    [InlineData("\\nonexistent\\")]
    //    [InlineData("\\nonexistent/")]
    //    [InlineData("/nonexistent\\")]
    //    public void Directory_ThatIsADirectSubDir_ReturnsNone_ForNonexistingDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.Directory(new KSPUrlIdentifier(dirname)).Any());
    //    }



    //    [Theory]
    //    [InlineData("subsubdir")]
    //    [InlineData("/subsubdir")]
    //    [InlineData("/subsubdir/")]
    //    [InlineData("\\subsubdir")]
    //    [InlineData("\\subsubdir\\")]
    //    [InlineData("/subsubdir\\")]
    //    [InlineData("\\subsubdir/")]
    //    public void DirectoryExists_ThatIsNotDirectSubDir_ReturnsFalse_WhenAccessedLikeSubDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.DirectoryExists(new KSPUrlIdentifier(dirname)));
    //    }



    //    [Theory]
    //    [InlineData("subsubdir")]
    //    [InlineData("/subsubdir")]
    //    [InlineData("/subsubdir/")]
    //    [InlineData("\\subsubdir")]
    //    [InlineData("\\subsubdir\\")]
    //    [InlineData("/subsubdir\\")]
    //    [InlineData("\\subsubdir/")]
    //    public void Directory_ThatIsNotDirectSubDir_ReturnsNone_WhenAccessingLikeSubDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.Directory(new KSPUrlIdentifier(dirname)).Any());
    //    }



    //    [Theory]
    //    [InlineData("subdir/subsubdir")]
    //    [InlineData("/subdir/subsubdir")]
    //    [InlineData("/subdir/subsubdir/")]
    //    [InlineData("subdir\\subsubdir")]
    //    [InlineData("\\subdir\\subsubdir")]
    //    [InlineData("\\subdir\\subsubdir\\")]
    //    [InlineData("/subdir/subsubdir\\")]
    //    [InlineData("\\subdir\\subsubdir/")]
    //    public void Directory_ThatIsNotDirectSubDir_ReturnsTrue_WhenAccessedLikeSubSubDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.DirectoryExists(new KSPUrlIdentifier(dirname)));
    //    }



    //    [Theory]
    //    [InlineData("subdir/subsubdir")]
    //    [InlineData("/subdir/subsubdir")]
    //    [InlineData("/subdir/subsubdir/")]
    //    [InlineData("subdir\\subsubdir")]
    //    [InlineData("\\subdir\\subsubdir")]
    //    [InlineData("\\subdir\\subsubdir\\")]
    //    [InlineData("/subdir/subsubdir\\")]
    //    [InlineData("\\subdir\\subsubdir/")]
    //    public void Directory_ThatIsNotDirectSubDir_ReturnsDir_WhenAccessedLikeSubSubDir(string dirname)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.Directory(new KSPUrlIdentifier(dirname)).Any());
    //    }







    //    [Theory]
    //    [InlineData("/.txt")]
    //    [InlineData("\\.txt")]
    //    [InlineData(".txt/")]
    //    [InlineData("txt/")]
    //    [InlineData(".txt\\")]
    //    [InlineData("txt\\")]
    //    public void Files_WithExtension_FromSameDir_BadExtensionName_DontMatchAgainstExisting(string badExtension)
    //    {
    //        var sut = Factory.Create();

    //        Assert.Empty(sut.Files(badExtension));
    //    }



    //    [Theory]
    //    [InlineData("txt", 1)]
    //    [InlineData(".txt", 1)]
    //    [InlineData("sub", 0)] // subsubdir
    //    [InlineData(".sub", 0)]
    //    [InlineData("none", 0)]
    //    [InlineData(".none", 0)]
    //    public void Files_WithExtension_FromGameData_ThatExist_ExpectedCount(string extension, int count)
    //    {
    //        var sut = Factory.Create();

    //        Assert.Equal(count, sut.Files(extension).Count());
    //    }



    //    [Fact]
    //    public void Files_NoExtension_FromGameData_FindsOne()
    //    {
    //        var sut = Factory.Create();

    //        Assert.Equal(1, sut.Files().Count());
    //    }



    //    [Theory]
    //    [InlineData(".none")]
    //    [InlineData("none")]
    //    [InlineData(".sub")]
    //    [InlineData("sub")]
    //    public void Files_WithExtension_ThatDoesntMatchAnyFiles_FromGameData_FindsNone(string extension)
    //    {
    //        var sut = Factory.Create();

    //        Assert.Empty(sut.Files(extension));
    //    }



    //    [Fact]
    //    public void RecursiveFiles_NoExtension_FromGameData_FindsThreeExampleFiles()
    //    {
    //        var sut = Factory.Create();

    //        Assert.Equal(3, sut.RecursiveFiles().Count());
    //    }



    //    [Fact]
    //    public void RecursiveFiles_NoExtension_FromSubDir_FindsTwoExampleFiles()
    //    {
    //        var sut = Factory.Create();

    //        Assert.Equal(2, sut.Directory(new KSPUrlIdentifier("subdir")).Single().RecursiveFiles().Count());
    //    }



    //    [Theory]
    //    [InlineData("subdir/subfile.txt")]
    //    [InlineData("subdir/subfile")]
    //    [InlineData("subdir\\subfile.txt")]
    //    [InlineData("subdir\\subfile")]
    //    [InlineData("subdir/subsubdir/subsubfile.sub")]
    //    [InlineData("subdir/subsubdir/subsubfile")]
    //    [InlineData("subdir\\subsubdir\\subsubfile.sub")]
    //    [InlineData("subdir\\subsubdir\\subsubfile")]
    //    [InlineData("/subdir/subfile.txt")]
    //    [InlineData("/subdir/subfile")]
    //    [InlineData("/subdir/subsubdir/subsubfile.sub")]
    //    [InlineData("/subdir/subsubdir/subsubfile")]
    //    [InlineData("\\subdir\\subfile.txt")]
    //    [InlineData("\\subdir\\subfile")]
    //    [InlineData("\\subdir\\subsubdir\\subsubfile.sub")]
    //    [InlineData("\\subdir\\subsubdir\\subsubfile")]
    //    public void Files_NoExtension_WithPathThatIncludesDirectory_FindsFileWhichExists(string url)
    //    {
    //        var sut = Factory.Create();

    //        Assert.True(sut.File(new KSPUrlIdentifier(url)).Any());
    //    }



    //    [Theory]
    //    [InlineData("subdir/noexist.txt")]
    //    [InlineData("subdir/noexist")]
    //    [InlineData("subdir\\noexist.txt")]
    //    [InlineData("subdir\\noexist")]
    //    [InlineData("subdir/subsubdir/noexist.sub")]
    //    [InlineData("subdir/subsubdir/noexist")]
    //    [InlineData("subdir\\subsubdir\\noexist.sub")]
    //    [InlineData("subdir\\subsubdir\\noexist")]
    //    [InlineData("/subdir/noexist.txt")]
    //    [InlineData("/subdir/noexist")]
    //    [InlineData("/subdir/subsubdir/noexist.sub")]
    //    [InlineData("/subdir/subsubdir/noexist")]
    //    [InlineData("\\subdir\\noexist.txt")]
    //    [InlineData("\\subdir\\noexist")]
    //    [InlineData("\\subdir\\subsubdir\\noexist.sub")]
    //    [InlineData("\\subdir\\subsubdir\\noexist")]
    //    public void Files_NoExtension_WithPathThatIncludesDirectory_DoesNotFindNonexistingFiles(string url)
    //    {
    //        var sut = Factory.Create();

    //        Assert.False(sut.File(new KSPUrlIdentifier(url)).Any());
    //    }
    //}
}
