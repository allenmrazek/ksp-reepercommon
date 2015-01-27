using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;

namespace UnitTests.FileSystem
{
    class KSPDirectory_Test
    {
        // Mocked UrlDirs (internal objects representing cached directories in KSP)
        private Mock<IUrlDir> _gameDataUrlDir;
        private Mock<IUrlDir> _subdirInGameData;

        // Mocked UrlFiles (like UrlDirs, but .. for files)
        private Mock<IUrlFile> _fileInGameData;
        private Mock<IUrlFile> _fileInSubdir;

        // Test environment
        // 
        // /GameData
        //     Files: 
        //          /GameData/test.txt
        //     Directories:
        //          /Subdir
        //              Files: /subfile.txt
        
        [SetUp]
        public void Init()
        {
            // file "test.txt" in GameData/
            _fileInGameData = new Mock<IUrlFile>();
            _fileInGameData.SetupGet(p => p.FullPath).Returns("C:/GameData/test.txt");
            _fileInGameData.SetupGet(p => p.Name).Returns("test");
            _fileInGameData.SetupGet(p => p.Extension).Returns("txt");
            _fileInGameData.SetupGet(p => p.Url).Returns("/test");


            // file "subfile.txt" in GameData/Subdir
            _fileInSubdir = new Mock<IUrlFile>();
            _fileInSubdir.SetupGet(p => p.FullPath).Returns("C:/GameData/Subdir/subfile.txt");
            _fileInSubdir.SetupGet(p => p.Name).Returns("subfile");
            _fileInSubdir.SetupGet(p => p.Extension).Returns("txt");
            _fileInSubdir.SetupGet(p => p.Url).Returns("/Subdir/subfile");


            // directory "GameData"
            _gameDataUrlDir = new Mock<IUrlDir>();
            _gameDataUrlDir.SetupGet(p => p.Name).Returns("GameData");
            _gameDataUrlDir.SetupGet(p => p.Url).Returns("/GameData");
            _gameDataUrlDir.SetupGet(p => p.Parent).Returns((IUrlDir)null);
            _gameDataUrlDir.SetupGet(p => p.Files).Returns(new[] {_fileInGameData.Object});
            _gameDataUrlDir.SetupGet(p => p.AllFiles).Returns(new[] {_fileInGameData.Object, _fileInSubdir.Object});

            // subdirectory "Subdir" in GameData/
            _subdirInGameData = new Mock<IUrlDir>();
            _subdirInGameData.SetupGet(p => p.Name).Returns("Subdir");
            _subdirInGameData.SetupGet(p => p.Url).Returns("/GameData/Subdir");
            _subdirInGameData.SetupGet(p => p.Files).Returns(new[] {_fileInSubdir.Object});
            _subdirInGameData.SetupGet(p => p.AllFiles).Returns(new[] {_fileInSubdir.Object});



            // set up relationship between gd and subdir
            _subdirInGameData.SetupGet(p => p.Parent).Returns(_gameDataUrlDir.Object);

            _gameDataUrlDir.SetupGet(p => p.Children).Returns(new[] {_subdirInGameData.Object});
            
        }



        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_FirstArgumentNull_ThrowsException()
        {
            var d = new KSPDirectory(null, new Mock<IUrlDir>().Object);
        }



        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Constructor_SecondArgumentNull_ThrowsException()
        {
            var d = new KSPDirectory(new Mock<IFileSystemFactory>().Object, null);
        }



        [Test]
        public void FileExists_InSameDir()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsTrue(gameData.FileExists("test.txt"));
            Assert.IsTrue(gameData.FileExists("/test.txt"));
            Assert.IsTrue(gameData.FileExists("test"));
            Assert.IsTrue(gameData.FileExists("/test"));
            Assert.IsTrue(gameData.FileExists("\\test.txt"));
            Assert.IsTrue(gameData.FileExists("\\test"));
        }


        [Test]
        public void FileExists_InSubDir()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsTrue(gameData.FileExists("Subdir/subfile.txt"));
            Assert.IsTrue(gameData.FileExists("/Subdir/subfile.txt"));
            Assert.IsTrue(gameData.FileExists("Subdir/subfile"));
            Assert.IsTrue(gameData.FileExists("/Subdir/subfile"));
            Assert.IsTrue(gameData.FileExists("\\Subdir\\subfile.txt"));
            Assert.IsTrue(gameData.FileExists("\\Subdir\\subfile"));
            Assert.IsTrue(gameData.FileExists("\\Subdir/subfile.txt"));
            Assert.IsTrue(gameData.FileExists("\\Subdir/subfile"));
            Assert.IsTrue(gameData.FileExists("/Subdir\\subfile.txt"));
            Assert.IsTrue(gameData.FileExists("/Subdir\\subfile"));
        }



        [Test]
        public void FileExists_InSameDir_WithNonexistentFile()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsFalse(gameData.FileExists("fake.txt"));
            Assert.IsFalse(gameData.FileExists("/fake.txt"));
            Assert.IsFalse(gameData.FileExists("fake"));
            Assert.IsFalse(gameData.FileExists("/fake"));
            Assert.IsFalse(gameData.FileExists("\\fake.txt"));
            Assert.IsFalse(gameData.FileExists("\\fake"));
        }



        [Test]
        public void FileExists_InSubDir_WithNonexistentFile()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsFalse(gameData.FileExists("Subdir/fake.txt"));
            Assert.IsFalse(gameData.FileExists("/Subdir/fake.txt"));
            Assert.IsFalse(gameData.FileExists("Subdir/fake"));
            Assert.IsFalse(gameData.FileExists("/Subdir/fake"));
            Assert.IsFalse(gameData.FileExists("\\Subdir\\fake.txt"));
            Assert.IsFalse(gameData.FileExists("\\Subdir\\fake"));
            Assert.IsFalse(gameData.FileExists("\\Subdir/fake.txt"));
            Assert.IsFalse(gameData.FileExists("\\Subdir/fake"));
            Assert.IsFalse(gameData.FileExists("/Subdir\\fake.txt"));
            Assert.IsFalse(gameData.FileExists("/Subdir\\fake"));
        }



        [Test]
        public void File_InSameDir()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsTrue(gameData.File("test.txt").Any());
            Assert.IsTrue(gameData.File("/test.txt").Any());
            Assert.IsTrue(gameData.File("test").Any());
            Assert.IsTrue(gameData.File("/test").Any());
            Assert.IsTrue(gameData.File("\\test.txt").Any());
            Assert.IsTrue(gameData.File("\\test").Any());
        }



        [Test]
        public void File_InSubDir()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsTrue(gameData.File("Subdir/subfile.txt").Any());
            Assert.IsTrue(gameData.File("/Subdir/subfile.txt").Any());
            Assert.IsTrue(gameData.File("Subdir/subfile").Any());
            Assert.IsTrue(gameData.File("/Subdir/subfile").Any());
            Assert.IsTrue(gameData.File("\\Subdir\\subfile.txt").Any());
            Assert.IsTrue(gameData.File("\\Subdir\\subfile").Any());
            Assert.IsTrue(gameData.File("\\Subdir/subfile.txt").Any());
            Assert.IsTrue(gameData.File("\\Subdir/subfile").Any());
            Assert.IsTrue(gameData.File("/Subdir\\subfile.txt").Any());
            Assert.IsTrue(gameData.File("/Subdir\\subfile").Any());
        }



        [Test]
        public void File_InSameDir_WithNonexistentFile()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsFalse(gameData.File("fake.txt").Any());
            Assert.IsFalse(gameData.File("/fake.txt").Any());
            Assert.IsFalse(gameData.File("fake").Any());
            Assert.IsFalse(gameData.File("/fake").Any());
            Assert.IsFalse(gameData.File("\\fake.txt").Any());
            Assert.IsFalse(gameData.File("\\fake").Any());
        }



        [Test]
        public void File_InSubDir_WithNonexistentFile()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsFalse(gameData.File("Subdir/fake.txt").Any());
            Assert.IsFalse(gameData.File("/Subdir/fake.txt").Any());
            Assert.IsFalse(gameData.File("Subdir/fake").Any());
            Assert.IsFalse(gameData.File("/Subdir/fake").Any());
            Assert.IsFalse(gameData.File("\\Subdir\\fake.txt").Any());
            Assert.IsFalse(gameData.File("\\Subdir\\fake").Any());
            Assert.IsFalse(gameData.File("\\Subdir/fake.txt").Any());
            Assert.IsFalse(gameData.File("\\Subdir/fake").Any());
            Assert.IsFalse(gameData.File("/Subdir\\fake.txt").Any());
            Assert.IsFalse(gameData.File("/Subdir\\fake").Any());
        }



        [Test]
        public void DirectoryExists_SubDirOfGameData()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsTrue(gameData.DirectoryExists("Subdir"));
            Assert.IsTrue(gameData.DirectoryExists("/Subdir"));
            Assert.IsTrue(gameData.DirectoryExists("\\Subdir"));
            Assert.IsTrue(gameData.DirectoryExists("/Subdir/"));
            Assert.IsTrue(gameData.DirectoryExists("\\Subdir\\"));
        }



        [Test]
        public void DirectoryExists_SubDirOfGameData_WithNonexistentDirectory()
        {
            var gameData = Make_Directory(_gameDataUrlDir);

            Assert.IsFalse(gameData.DirectoryExists("Fake"));
            Assert.IsFalse(gameData.DirectoryExists("/Fake"));
            Assert.IsFalse(gameData.DirectoryExists("\\Fake"));
            Assert.IsFalse(gameData.DirectoryExists("/Fake/"));
            Assert.IsFalse(gameData.DirectoryExists("\\Fake\\"));
        }






        private static IDirectory Make_Directory(Mock<IUrlDir> urlDir)
        {
            var fsf = new Mock<IFileSystemFactory>();

            fsf.Setup(t => t.GetFile(It.IsAny<IDirectory>(), It.IsAny<IUrlFile>()))
                .Returns((IDirectory dir, IUrlFile f) => { 
                    var mockFile = new Mock<IFile>();

                    mockFile.SetupGet(p => p.Directory).Returns(dir);
                    mockFile.SetupGet(p => p.Extension).Returns(f.Extension);
                    mockFile.SetupGet(p => p.Name).Returns(f.Name);
                    mockFile.SetupGet(p => p.UrlFile).Returns(f);
                    mockFile.SetupGet(p => p.Url).Returns(f.Url);
                    mockFile.SetupGet(p => p.FileName)
                        .Returns(f.Name + "." + f.Extension);
                    mockFile.SetupGet(p => p.FullPath).Returns(f.FullPath);
                    mockFile.SetupGet(p => p.Info).Returns(Maybe<FileInfo>.None);

                    return mockFile.Object;
                });

            fsf.Setup(t => t.GetDirectory(It.IsAny<IUrlDir>()))
                .Returns((IUrlDir dir) => new KSPDirectory(fsf.Object, dir));

            var mockedDirectory = new KSPDirectory(fsf.Object, urlDir.Object);

            return mockedDirectory;
        }
    }
}
