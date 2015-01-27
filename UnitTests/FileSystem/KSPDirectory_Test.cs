using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.FileSystem
{
    public class KSPDirectory_Test
    {
        internal static class Factory
        {
            static IUrlFile CreateFileNoSubdir()
            {
                var file = Substitute.For<IUrlFile>();

                file.Name.Returns("file");
                file.Extension.Returns("txt");
                file.FullPath.Returns("C:/GameData/file.txt");
                file.Url.Returns("/file");

                return file;
            }

            static IUrlFile CreateFileInSubdir()
            {
                var file = Substitute.For<IUrlFile>();

                file.Name.Returns("subfile");
                file.Extension.Returns("txt");
                file.FullPath.Returns("C:/GameData/subdir/subfile.txt");
                file.Url.Returns("/subdir/subfile");
                
                return file;
            }

            static IUrlDir CreateUrlDirGameData()
            {
                var dir = Substitute.For<IUrlDir>();

                var fileNoSubDir = CreateFileNoSubdir();
                var fileSubDir = CreateFileInSubdir();

                dir.Name.Returns("GameData");
                dir.Parent.Returns((IUrlDir)null);
                dir.FullPath.Returns("C:/GameData");
                dir.Files.Returns(new[] { fileNoSubDir });
                dir.AllFiles.Returns(new[] { fileNoSubDir, fileSubDir });
                dir.Url.Returns("/");

                var child = Substitute.For<IUrlDir>();
                child.Name.Returns("subdir");
                child.Parent.Returns((IUrlDir)null);
                child.FullPath.Returns("C:/GameData/subdir");
                child.Files.Returns(new[] { fileSubDir });
                child.AllFiles.Returns(new[] { fileSubDir });
                child.Url.Returns("/");

                dir.Children.Returns(new[] {child});

                return dir;
            }

            static IFileSystemFactory CreateFileSystemFactory()
            {
                var fsf = Substitute.For<IFileSystemFactory>();

                fsf.GetDirectory(Arg.Any<IUrlDir>()).ReturnsForAnyArgs(d => new KSPDirectory(fsf, d.Arg<IUrlDir>()));

                return fsf;
            }

            public static KSPDirectory Create()
            {
                return new KSPDirectory(CreateFileSystemFactory(), CreateUrlDirGameData());
            }
        }



        [Fact]
        void Constructor_FirstArgumentNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new KSPDirectory(null, Substitute.For<IUrlDir>()));
        }



        [Fact]
        void Constructor_SecondArgumentNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new KSPDirectory(Substitute.For<IFileSystemFactory>(), null));
        }



        [Theory]
        [InlineData("file.txt")]
        [InlineData("/file.txt")]
        [InlineData("file")]
        [InlineData("/file")]
        [InlineData("\\file.txt")]
        [InlineData("\\file")]
        void FileExists_InSameDir_ReturnsTrue_WithExistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.True(sut.FileExists(filename));
        }



        [Theory]
        [InlineData("subdir/subfile.txt")]
        [InlineData("/subdir/subfile.txt")]
        [InlineData("subdir/subfile")]
        [InlineData("/subdir/subfile")]
        [InlineData("\\subdir\\subfile.txt")]
        [InlineData("\\subdir\\subfile")]
        [InlineData("\\subdir/subfile.txt")]
        [InlineData("\\subdir/subfile")]
        [InlineData("/subdir\\subfile.txt")]
        [InlineData("/subdir\\subfile")]
        public void FileExists_InSubDir_ReturnsTrue_WithExistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.True(sut.FileExists(filename));
        }



        [Theory]
        [InlineData("doesntExist.txt")]
        [InlineData("/doesntExist.txt")]
        [InlineData("doesntExist")]
        [InlineData("/doesntExist")]
        [InlineData("\\doesntExist.txt")]
        [InlineData("\\doesntExist")]
        void FileExists_InSameDir_ReturnsFalse_WithNonExistentFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.FileExists(filename));
        }



        //[Test]
        //public void FileExists_InSameDir_WithNonexistentFile()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsFalse(gameData.FileExists("fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("/fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("fake"));
        //    Assert.IsFalse(gameData.FileExists("/fake"));
        //    Assert.IsFalse(gameData.FileExists("\\fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("\\fake"));
        //}



        //[Test]
        //public void FileExists_InSubDir_WithNonexistentFile()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsFalse(gameData.FileExists("Subdir/fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("/Subdir/fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("Subdir/fake"));
        //    Assert.IsFalse(gameData.FileExists("/Subdir/fake"));
        //    Assert.IsFalse(gameData.FileExists("\\Subdir\\fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("\\Subdir\\fake"));
        //    Assert.IsFalse(gameData.FileExists("\\Subdir/fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("\\Subdir/fake"));
        //    Assert.IsFalse(gameData.FileExists("/Subdir\\fake.txt"));
        //    Assert.IsFalse(gameData.FileExists("/Subdir\\fake"));
        //}



        //[Test]
        //public void File_InSameDir()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsTrue(gameData.File("test.txt").Any());
        //    Assert.IsTrue(gameData.File("/test.txt").Any());
        //    Assert.IsTrue(gameData.File("test").Any());
        //    Assert.IsTrue(gameData.File("/test").Any());
        //    Assert.IsTrue(gameData.File("\\test.txt").Any());
        //    Assert.IsTrue(gameData.File("\\test").Any());
        //}



        //[Test]
        //public void File_InSubDir()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsTrue(gameData.File("Subdir/subfile.txt").Any());
        //    Assert.IsTrue(gameData.File("/Subdir/subfile.txt").Any());
        //    Assert.IsTrue(gameData.File("Subdir/subfile").Any());
        //    Assert.IsTrue(gameData.File("/Subdir/subfile").Any());
        //    Assert.IsTrue(gameData.File("\\Subdir\\subfile.txt").Any());
        //    Assert.IsTrue(gameData.File("\\Subdir\\subfile").Any());
        //    Assert.IsTrue(gameData.File("\\Subdir/subfile.txt").Any());
        //    Assert.IsTrue(gameData.File("\\Subdir/subfile").Any());
        //    Assert.IsTrue(gameData.File("/Subdir\\subfile.txt").Any());
        //    Assert.IsTrue(gameData.File("/Subdir\\subfile").Any());
        //}



        //[Test]
        //public void File_InSameDir_WithNonexistentFile()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsFalse(gameData.File("fake.txt").Any());
        //    Assert.IsFalse(gameData.File("/fake.txt").Any());
        //    Assert.IsFalse(gameData.File("fake").Any());
        //    Assert.IsFalse(gameData.File("/fake").Any());
        //    Assert.IsFalse(gameData.File("\\fake.txt").Any());
        //    Assert.IsFalse(gameData.File("\\fake").Any());
        //}



        //[Test]
        //public void File_InSubDir_WithNonexistentFile()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsFalse(gameData.File("Subdir/fake.txt").Any());
        //    Assert.IsFalse(gameData.File("/Subdir/fake.txt").Any());
        //    Assert.IsFalse(gameData.File("Subdir/fake").Any());
        //    Assert.IsFalse(gameData.File("/Subdir/fake").Any());
        //    Assert.IsFalse(gameData.File("\\Subdir\\fake.txt").Any());
        //    Assert.IsFalse(gameData.File("\\Subdir\\fake").Any());
        //    Assert.IsFalse(gameData.File("\\Subdir/fake.txt").Any());
        //    Assert.IsFalse(gameData.File("\\Subdir/fake").Any());
        //    Assert.IsFalse(gameData.File("/Subdir\\fake.txt").Any());
        //    Assert.IsFalse(gameData.File("/Subdir\\fake").Any());
        //}



        //[Test]
        //public void DirectoryExists_SubDirOfGameData()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsTrue(gameData.DirectoryExists("Subdir"));
        //    Assert.IsTrue(gameData.DirectoryExists("/Subdir"));
        //    Assert.IsTrue(gameData.DirectoryExists("\\Subdir"));
        //    Assert.IsTrue(gameData.DirectoryExists("/Subdir/"));
        //    Assert.IsTrue(gameData.DirectoryExists("\\Subdir\\"));
        //}



        //[Test]
        //public void DirectoryExists_SubDirOfGameData_WithNonexistentDirectory()
        //{
        //    var gameData = Make_Directory(_gameDataUrlDir);

        //    Assert.IsFalse(gameData.DirectoryExists("Fake"));
        //    Assert.IsFalse(gameData.DirectoryExists("/Fake"));
        //    Assert.IsFalse(gameData.DirectoryExists("\\Fake"));
        //    Assert.IsFalse(gameData.DirectoryExists("/Fake/"));
        //    Assert.IsFalse(gameData.DirectoryExists("\\Fake\\"));
        //}






        //private static IDirectory Make_Directory(Mock<IUrlDir> urlDir)
        //{
        //    var fsf = new Mock<IFileSystemFactory>();

        //    fsf.Setup(t => t.GetFile(It.IsAny<IDirectory>(), It.IsAny<IUrlFile>()))
        //        .Returns((IDirectory dir, IUrlFile f) => { 
        //            var mockFile = new Mock<IFile>();

        //            mockFile.SetupGet(p => p.Directory).Returns(dir);
        //            mockFile.SetupGet(p => p.Extension).Returns(f.Extension);
        //            mockFile.SetupGet(p => p.Name).Returns(f.Name);
        //            mockFile.SetupGet(p => p.UrlFile).Returns(f);
        //            mockFile.SetupGet(p => p.Url).Returns(f.Url);
        //            mockFile.SetupGet(p => p.FileName)
        //                .Returns(f.Name + "." + f.Extension);
        //            mockFile.SetupGet(p => p.FullPath).Returns(f.FullPath);
        //            mockFile.SetupGet(p => p.Info).Returns(Maybe<FileInfo>.None);

        //            return mockFile.Object;
        //        });

        //    fsf.Setup(t => t.GetDirectory(It.IsAny<IUrlDir>()))
        //        .Returns((IUrlDir dir) => new KSPDirectory(fsf.Object, dir));

        //    var mockedDirectory = new KSPDirectory(fsf.Object, urlDir.Object);

        //    return mockedDirectory;
        //}
    }
}
