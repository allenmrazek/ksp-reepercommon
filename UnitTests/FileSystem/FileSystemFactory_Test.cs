using System;
using NUnit.Framework;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using Moq;
using ReeperCommon.FileSystem.Implementations;

namespace UnitTests.FileSystem
{
    [TestFixture]
    public class FileSystemFactory_Test
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArgs_ThrowsNullException()
        {
            var fsf = new KSPFileSystemFactory(null);
        }



        [Test]
        public void GetGameDataDirectory_Correct()
        {
            var gdMock = new Mock<IDirectory>();
            var fsFactory = new KSPFileSystemFactory(gdMock.Object);

            Assert.AreEqual(gdMock.Object, fsFactory.GetGameDataDirectory());
        }
    }
}
