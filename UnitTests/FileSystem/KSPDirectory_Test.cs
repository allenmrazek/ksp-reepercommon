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
        private static class Factory
        {
            static IUrlFile CreateFileNoSubdirectory()
            {
                var file = Substitute.For<IUrlFile>();

                file.Name.Returns("file");
                file.Extension.Returns("txt");
                file.FullPath.Returns("C:/GameData/file.txt");
                file.Url.Returns("/file");

                return file;
            }

            static IUrlFile CreateFileInSubdirectory()
            {
                var file = Substitute.For<IUrlFile>();

                file.Name.Returns("subfile");
                file.Extension.Returns("txt");
                file.FullPath.Returns("C:/GameData/subdir/subfile.txt");
                file.Url.Returns("/subdir/subfile");
                
                return file;
            }

            static IUrlFile CreateFileInSub_Subdirectory()
            {
                var file = Substitute.For<IUrlFile>();

                file.Name.Returns("subsubfile");
                file.Extension.Returns("sub");
                file.FullPath.Returns("C:/GameData/subdir/subsubdir/subsubfile.txt");
                file.Url.Returns("/subdir/subsubdir/subsubfile");

                return file;
            }

            static IUrlDir CreateUrlDirGameData()
            {
                var gdDir = Substitute.For<IUrlDir>();

                var fileNoSubDir = CreateFileNoSubdirectory();
                var fileSubDir = CreateFileInSubdirectory();
                var fileSubSubDir = CreateFileInSub_Subdirectory();

                gdDir.Name.Returns("GameData");
                gdDir.Parent.Returns((IUrlDir)null);
                gdDir.FullPath.Returns("C:/GameData");
                gdDir.Files.Returns(new[] { fileNoSubDir });
                gdDir.AllFiles.Returns(new[] { fileNoSubDir, fileSubDir, fileSubSubDir });
                gdDir.Url.Returns("/");

                var subdir = Substitute.For<IUrlDir>();
                subdir.Name.Returns("subdir");
                subdir.Parent.Returns(gdDir);
                subdir.FullPath.Returns("C:/GameData/subdir");
                subdir.Files.Returns(new[] { fileSubDir });
                subdir.AllFiles.Returns(new[] { fileSubDir, fileSubSubDir });
                subdir.Url.Returns("/subdir");

                var subsubdir = Substitute.For<IUrlDir>();
                subsubdir.Name.Returns("subsubdir");
                subsubdir.Parent.Returns(subdir);
                subsubdir.FullPath.Returns("C:/GameData/subdir/subsubdir");
                subsubdir.Url.Returns("/subdir/subsubdir/");
                subsubdir.Files.Returns(new[] {fileSubSubDir});
                subsubdir.AllFiles.Returns(new[] {fileSubSubDir});

                gdDir.Children.Returns(new[] {subdir});
                subdir.Children.Returns(new[] {subsubdir});

                return gdDir;
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
        public void FileExists_InExistingSubDir_ReturnsTrue_WithExistingFile(string filename)
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



        [Theory]
        [InlineData("subdir/doesntExist.txt")]
        [InlineData("/subdir/doesntExist.txt")]
        [InlineData("subdir/doesntExist")]
        [InlineData("/subdir/doesntExist")]
        [InlineData("\\subdir\\doesntExist.txt")]
        [InlineData("\\subdir\\doesntExist")]
        [InlineData("\\subdir/doesntExist.txt")]
        [InlineData("\\subdir/doesntExist")]
        [InlineData("/subdir\\doesntExist.txt")]
        [InlineData("/subdir\\doesntExist")]
        void FileExists_InExistingSubDir_ReturnsFalse_WithNonExistentFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.FileExists(filename));
        }


        [Theory]
        [InlineData("badsubdir/doesntExist.txt")]
        [InlineData("/badsubdir/doesntExist.txt")]
        [InlineData("badsubdir/doesntExist")]
        [InlineData("/badsubdir/doesntExist")]
        [InlineData("\\badsubdir\\doesntExist.txt")]
        [InlineData("\\badsubdir\\doesntExist")]
        [InlineData("\\badsubdir/doesntExist.txt")]
        [InlineData("\\badsubdir/doesntExist")]
        [InlineData("/badsubdir\\doesntExist.txt")]
        [InlineData("/badsubdir\\doesntExist")]
        void FileExists_InNonexistingSubDir_ReturnsFalse_WithNonExistentFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.FileExists(filename));
        }



        [Theory]
        [InlineData("subdir/subsubdir/subsubfile.sub")]
        [InlineData("subdir/subsubdir/subsubfile")]
        [InlineData("subdir/subsubdir\\subsubfile.sub")]
        [InlineData("subdir/subsubdir\\subsubfile")]
        [InlineData("subdir\\subsubdir\\subsubfile.sub")]
        [InlineData("subdir\\subsubdir\\subsubfile")]
        [InlineData("subdir\\subsubdir/subsubfile.sub")]
        [InlineData("subdir\\subsubdir/subsubfile")]
        [InlineData("subdir/subsubdir\\subsubfile.sub")]
        [InlineData("subdir/subsubdir\\subsubfile")]
        [InlineData("/subdir/subsubdir/subsubfile.sub")]
        [InlineData("/subdir/subsubdir/subsubfile")]
        [InlineData("\\subdir\\subsubdir\\subsubfile.sub")]
        [InlineData("\\subdir\\subsubdir\\subsubfile")]
        void FileExists_InSubSubDir_ReturnsTrue_WithExistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.True(sut.FileExists(filename));
        }



        [Theory]
        [InlineData("file.txt")]
        [InlineData("/file.txt")]
        [InlineData("file")]
        [InlineData("/file")]
        [InlineData("\\file.txt")]
        [InlineData("\\file")]
        public void File_InSameDir_ReturnsFileWhichExists(string filename)
        {
            var sut = Factory.Create();

            Assert.True(sut.File(filename).Any());
        }



        [Theory]
        [InlineData("doesntExist.txt")]
        [InlineData("/doesntExist.txt")]
        [InlineData("doesntExist")]
        [InlineData("/doesntExist")]
        [InlineData("\\doesntExist.txt")]
        [InlineData("\\doesntExist")]
        public void File_InSameDir_ReturnsNone_ForNonexistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.File(filename).Any());
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
        public void File_InExistingSubDir_ReturnsFileWhichExists(string filename)
        {
            var sut = Factory.Create();

            Assert.True(sut.File(filename).Any());
        }



        [Theory]
        [InlineData("subdir/doesntExist.txt")]
        [InlineData("/subdir/doesntExist.txt")]
        [InlineData("subdir/doesntExist")]
        [InlineData("/subdir/doesntExist")]
        [InlineData("\\subdir\\doesntExist.txt")]
        [InlineData("\\subdir\\doesntExist")]
        [InlineData("\\subdir/doesntExist.txt")]
        [InlineData("\\subdir/doesntExist")]
        [InlineData("/subdir\\doesntExist.txt")]
        [InlineData("/subdir\\doesntExist")]
        public void File_InExistingSubDir_ReturnsNone_ForNonexistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.File(filename).Any());
        }



        [Theory]
        [InlineData("badsubdir/doesntExist.txt")]
        [InlineData("/badsubdir/doesntExist.txt")]
        [InlineData("badsubdir/doesntExist")]
        [InlineData("/badsubdir/doesntExist")]
        [InlineData("\\badsubdir\\doesntExist.txt")]
        [InlineData("\\badsubdir\\doesntExist")]
        [InlineData("\\badsubdir/doesntExist.txt")]
        [InlineData("\\badsubdir/doesntExist")]
        [InlineData("/badsubdir\\doesntExist.txt")]
        [InlineData("/badsubdir\\doesntExist")]
        public void File_InNonexistingSubDir_ReturnsNone_ForNonexistingFile(string filename)
        {
            var sut = Factory.Create();

            Assert.False(sut.File(filename).Any());
        }



        [Theory]
        [InlineData("subdir")]
        [InlineData("/subdir")]
        [InlineData("/subdir/")]
        [InlineData("\\subdir")]
        [InlineData("\\subdir\\")]
        [InlineData("\\subdir/")]
        [InlineData("/subdir\\")]
        public void DirectoryExists_ThatIsADirectSubDir_ReturnsTrue_ForExistingDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.True(sut.DirectoryExists(dirname));
        }



        [Theory]
        [InlineData("subdir")]
        [InlineData("/subdir")]
        [InlineData("/subdir/")]
        [InlineData("\\subdir")]
        [InlineData("\\subdir\\")]
        [InlineData("\\subdir/")]
        [InlineData("/subdir\\")]
        public void Directory_ThatIsADirectSubDir_ReturnsValid_ForExistingDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.True(sut.Directory(dirname).Any());
        }



        [Theory]
        [InlineData("nonexistent")]
        [InlineData("/nonexistent")]
        [InlineData("/nonexistent/")]
        [InlineData("\\nonexistent")]
        [InlineData("\\nonexistent\\")]
        [InlineData("\\nonexistent/")]
        [InlineData("/nonexistent\\")]
        public void DirectoyrExists_ThatIsADirectSubDir_ReturnsNone_ForNonexistingDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.False(sut.DirectoryExists(dirname));
        }



        [Theory]
        [InlineData("nonexistent")]
        [InlineData("/nonexistent")]
        [InlineData("/nonexistent/")]
        [InlineData("\\nonexistent")]
        [InlineData("\\nonexistent\\")]
        [InlineData("\\nonexistent/")]
        [InlineData("/nonexistent\\")]
        public void Directory_ThatIsADirectSubDir_ReturnsNone_ForNonexistingDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.False(sut.Directory(dirname).Any());
        }



        [Theory]
        [InlineData("subsubdir")]
        [InlineData("/subsubdir")]
        [InlineData("/subsubdir/")]
        [InlineData("\\subsubdir")]
        [InlineData("\\subsubdir\\")]
        [InlineData("/subsubdir\\")]
        [InlineData("\\subsubdir/")]
        public void DirectoryExists_ThatIsNotDirectSubDir_ReturnsFalse_WhenAccessedLikeSubDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.False(sut.DirectoryExists(dirname));
        }



        [Theory]
        [InlineData("subsubdir")]
        [InlineData("/subsubdir")]
        [InlineData("/subsubdir/")]
        [InlineData("\\subsubdir")]
        [InlineData("\\subsubdir\\")]
        [InlineData("/subsubdir\\")]
        [InlineData("\\subsubdir/")]
        public void Directory_ThatIsNotDirectSubDir_ReturnsNone_WhenAccessingLikeSubDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.False(sut.Directory(dirname).Any());
        }



        [Theory]
        [InlineData("subdir/subsubdir")]
        [InlineData("/subdir/subsubdir")]
        [InlineData("/subdir/subsubdir/")]
        [InlineData("subdir\\subsubdir")]
        [InlineData("\\subdir\\subsubdir")]
        [InlineData("\\subdir\\subsubdir\\")]
        [InlineData("/subdir/subsubdir\\")]
        [InlineData("\\subdir\\subsubdir/")]
        public void Directory_ThatIsNotDirectSubDir_ReturnsTrue_WhenAccessedLikeSubSubDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.True(sut.DirectoryExists(dirname));
        }



        [Theory]
        [InlineData("subdir/subsubdir")]
        [InlineData("/subdir/subsubdir")]
        [InlineData("/subdir/subsubdir/")]
        [InlineData("subdir\\subsubdir")]
        [InlineData("\\subdir\\subsubdir")]
        [InlineData("\\subdir\\subsubdir\\")]
        [InlineData("/subdir/subsubdir\\")]
        [InlineData("\\subdir\\subsubdir/")]
        public void Directory_ThatIsNotDirectSubDir_ReturnsDir_WhenAccessedLikeSubSubDir(string dirname)
        {
            var sut = Factory.Create();

            Assert.True(sut.Directory(dirname).Any());
        }


        
    }
}
