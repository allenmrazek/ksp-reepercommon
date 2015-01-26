using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;

namespace UnitTests.FileSystem
{
    class KSPDirectory_Test
    {
        public void Init()
        {
            //mockFile.Setup(t => t.FullPath).Returns("C:/GameData/test.txt");
            //mockFile.Setup(t => t.FileName).Returns("test.txt");
            //mockFile.Setup(t => t.Extension).Returns("txt");
            //mockFile.Setup(t => t.Directory).Returns(mockDirectory.Object);
            //mockFile.Setup(t => t.Name).Returns("test");
            //mockFile.Setup(t => t.Url).Returns("/test");
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
        public void File_InSameDir()
        {
            /*
            testfile.txt
            /subdir/subfile.txt
            */
            var first = Make_UrlFile("testfile", "txt", "/testfile", "C:/GameData/testfile.txt");
            var second = Make_UrlFile("subfile", "txt", "/subdir/subfile", "C:/GameData/subdir/subfile.txt");




            var fsf = new Mock<IFileSystemFactory>();

            fsf.Setup(t => t.GetFile(It.IsAny<IDirectory>(), It.IsAny<IUrlFile>()))
                .Returns((IDirectory dir, IUrlFile f) => new KSPFile(dir, f));

            fsf.Setup(t => t.GetDirectory(It.IsAny<IUrlDir>()))
                .Returns((IUrlDir dir) => new KSPDirectory(fsf.Object, dir));


            var urldirSub = Make_UrlDir("subdir", "/subdir", "C:/GameData/subdir/", new[] { second.Object },
                new[] { second.Object }, Enumerable.Empty<IUrlDir>());

            var urldirBase = Make_UrlDir("GameData", "/", "C:/GameData", new[] {first.Object}, new[] {first.Object, second.Object}, new[]{urldirSub.Object});
            



            var topDir = new KSPDirectory(fsf.Object, urldirBase.Object);
            var subDir = new KSPDirectory(fsf.Object, urldirSub.Object);


            // file in main dir
            Assert.IsNotNull(topDir.File("testfile"));
            Assert.IsNotNull(topDir.File("/testfile"));
            Assert.IsNotNull(topDir.File("testfile.txt"));
            Assert.IsNotNull(topDir.File("/testfile.txt"));
            Assert.IsNotNull(topDir.File("\\testfile"));
            Assert.IsNotNull(topDir.File("\\testfile.txt"));

            // file in subdir, accessed from top dir
            Assert.IsNotNull(topDir.File("subdir/subfile"));
            Assert.IsNotNull(topDir.File("/subdir/subfile"));
            Assert.IsNotNull(topDir.File("subdir/subfile.txt"));
            Assert.IsNotNull(topDir.File("/subdir/subfile.txt"));
            Assert.IsNotNull(topDir.File("subdir\\subfile"));
            Assert.IsNotNull(topDir.File("\\subdir\\subfile"));
            Assert.IsNotNull(topDir.File("subdir\\subfile.txt"));
            Assert.IsNotNull(topDir.File("\\subdir\\subfile.txt"));

            Assert.IsTrue(topDir.Directory("subdir").Any());
            Assert.IsTrue(topDir.Directory("/subdir").Any());
            Assert.IsTrue(topDir.Directory("/subdir/").Any());
            Assert.IsTrue(topDir.Directory("\\subdir").Any());
            Assert.IsTrue(topDir.Directory("\\subdir\\").Any());

            Assert.IsFalse(topDir.Directory("invalid").Any());
            Assert.IsFalse(topDir.Directory("/invalid").Any());
            Assert.IsFalse(topDir.Directory("/invalid/").Any());
            Assert.IsFalse(topDir.Directory("\\invalid").Any());
            Assert.IsFalse(topDir.Directory("\\invalid\\").Any());

        }




        private static Mock<IUrlFile> Make_UrlFile(
            string name,
            string extension,
            string url,
            string fullpath)
        {
            var mock = new Mock<IUrlFile>();

            mock.SetupGet(t => t.Name).Returns(name);
            mock.SetupGet(f => f.FullPath).Returns(fullpath);
            mock.SetupGet(f => f.Url).Returns(url);
            mock.SetupGet(f => f.Extension).Returns(extension);

            return mock;
        }
           

        private static Mock<IUrlDir> Make_UrlDir(
            string name,
            string url,
            string fullpath,
            IEnumerable<IUrlFile> files,
            IEnumerable<IUrlFile> allFiles,
            IEnumerable<IUrlDir> children)
        {
            var mock = new Mock<IUrlDir>();
            
            mock.SetupGet(p => p.Name).Returns(name);
            //mock.SetupGet(p => p.Parent).Returns(null);
            mock.SetupGet(p => p.Url).Returns(url);
            mock.SetupGet(p => p.FullPath).Returns(fullpath);
            mock.SetupGet(p => p.Files).Returns(files);
            mock.SetupGet(p => p.AllFiles).Returns(allFiles);
            mock.SetupGet(p => p.Children).Returns(children);
                   
            return mock;
        }
    }
}
