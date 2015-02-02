//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Moq;
//using NUnit.Framework;
//using ReeperCommon.FileSystem;
//using ReeperCommon.FileSystem.Factories;
//using ReeperCommon.FileSystem.Implementations;

//namespace UnitTests.FileSystem
//{
//    [TestFixture]
//    public class KSPFile_Test
//    {
//        private IFakeDirectory testDirectory;
//        private IUrlFile testUrlFile;

//        [SetUp]
//        public void Init()
//        {
//            var mockDirectory = new Mock<IFakeDirectory>();
//            var mockUrlFile = new Mock<IUrlFile>();


//            mockUrlFile.Setup(t => t.Name).Returns("Test");
//            mockUrlFile.Setup(t => t.Url).Returns("/GameData/Test");
//            mockUrlFile.Setup(t => t.FullPath).Returns("C:/GameData/Test.txt");
//            mockUrlFile.Setup(t => t.Extension).Returns("txt");



//            testDirectory = mockDirectory.Object;
//            testUrlFile = mockUrlFile.Object;
//        }



//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Constructor_FirstParam_NullArgument_ThrowsException()
//        {
//            var f = new KSPFile(null, new Mock<IUrlFile>().Object);
//        }



//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Constructor_SecondParam_NullArgument_ThrowsException()
//        {
//            var f = new KSPFile(new Mock<IFakeDirectory>().Object, null);
//        }


//        [Test]
//        public void Properties_ReturnsCorrectReferences()
//        {
//            var dir = new Mock<IFakeDirectory>();
//            var urlFile = new Mock<IUrlFile>();

//            var f = new KSPFile(dir.Object, urlFile.Object);

//            var getDir = f.FakeDirectory;
//            var getUrlf = f.UrlFile;

//            Assert.AreSame(dir.Object, getDir);
//            Assert.AreSame(urlFile.Object, getUrlf);
//        }


//        public void Properties_UsesCorrect()
//        {
//            var mockUrlFile = new Mock<IUrlFile>();
//            var mockDirectory = new Mock<IFakeDirectory>();

//            mockUrlFile.SetupProperty(t => t.Name, "Test");
//            mockUrlFile.SetupProperty(t => t.Url, "/GameData/Test");
//            mockUrlFile.SetupProperty(t => t.FullPath, "C:/GameData/Test.txt");
//            mockUrlFile.SetupProperty(t => t.Extension, "txt");

//            var f = new KSPFile(mockDirectory.Object, mockUrlFile.Object);

//            var ext = f.Extension;
//            var name = f.Name;
//            var filename = f.FileName;
//            var url = f.Url;

//            mockUrlFile.VerifyGet(t => t.Extension, Times.Once());
//            mockUrlFile.VerifyGet(t => t.Name, Times.Once());
            
//            mockUrlFile.VerifyGet(t => t.Url, Times.Once());
            
//        }
//    }
//}
