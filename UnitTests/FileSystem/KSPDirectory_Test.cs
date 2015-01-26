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


        public void File_InSameDir()
        {
            /*
            testfile.txt
            /subdir/subfile.txt
            */
            var first = Make_UrlFile("testfile", "txt", "/testfile", "C:/GameData/testfile.txt");
            var second = Make_UrlFile("subfile", "txt", "/subdir/subfile", "C:/GameData/subdir/subfile.txt");

            //var root = Make_UrlDir("GameData", "", "C:/GameData/",
            //    new []
            //    {
            //        new Mock<IFile>().SetupProperty(p => p.UrlFile, first).Object
            //    },
            //    new []
            //    {
            //        new Mock<IFile>().SetupProperty(p => p.UrlFile, first).Object
            //    });
  



            var fsf = new Mock<IFileSystemFactory>();

            fsf.Setup(t => t.GetFile(It.IsAny<IDirectory>(), It.IsAny<IUrlFile>()))
                .Returns((IDirectory dir, IUrlFile f) => new KSPFile(dir, f));

            //var mainFile = new KSPFile(new KSPDirectory(fsf, 
            //uf.SetupProperty(t => t.Files, new[]
            //{
            //    new Mock<IFile>().SetupProperty(p => p.Url
            //});


            //var d = new KSPDirectory(new Mock<IFileSystemFactory>().Object, uf.Object);
        }




        private static Mock<IUrlFile> Make_UrlFile(
            string name,
            string extension,
            string url,
            string fullpath)
        {
            return new Mock<IUrlFile>()
                .SetupProperty(f => f.Name, name)
                .SetupProperty(f => f.FullPath, fullpath)
                .SetupProperty(f => f.Url, url)
                .SetupProperty(f => f.Extension, extension);
        }
           

        private static Mock<IUrlDir> Make_UrlDir(
            string name,
            string url,
            string fullpath,
            IEnumerable<IFile> files,
            IEnumerable<IFile> allFiles)
        {
            return new Mock<IUrlDir>()
                .SetupProperty(p => p.Name, name)
                .SetupProperty(p => p.Parent, null)
                .SetupProperty(p => p.Url, url)
                .SetupProperty(p => p.FullPath, fullpath)
                .SetupProperty(p => p.Files, files)
                .SetupProperty(p => p.AllFiles, allFiles);
        }
    }
}
