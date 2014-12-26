using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace ReeperCommon.Tests
{
    [TestSuite]
    class Test_KSPFile
    {
        private IDirectory _directory;

        [TestSetup]
        void DoSetup()
        {
            _directory = new KSPDirectory(new KSPFileFactory());
        }


        [Test]
        void Found_BYOSC_File_FromSameFolder_WithExtension()
        {
            Assert.IsTrue(_directory.Directory("BuildYourOwnSpaceCenter").File("BuildYourOwnSpaceCenter.dll") != null);
        }



        [Test]
        void Found_BYOSC_File_FromSameFolder_WithoutExtension()
        {
            Assert.IsTrue(_directory.Directory("BuildYourOwnSpaceCenter").File("BuildYourOwnSpaceCenter") != null);
        }




        [Test]
        void Found_BYOSC_File_FromGameData_NoPrecedingSlash_WithExtension()
        {
            Assert.IsTrue(_directory.File("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter.dll") != null);
        }


        [Test]
        void Found_BYOSC_File_FromGameData_NoPrecedingSlashWithoutExtension()
        {
            Assert.IsTrue(_directory.File("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter") != null);
        }


        [Test]
        void Found_BYOSC_File_FromGameData_PrecedingSlash_WithExtension()
        {
            Assert.IsTrue(_directory.File("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter.dll") != null);
        }


        [Test]
        void Found_BYOSC_File_FromGameData_PrecedingSlash_WithoutExtension()
        {
            Assert.IsTrue(_directory.File("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter") != null);
        }


        [Test]
        void File_Exists_Extension()
        {
            Assert.IsTrue(_directory.FileExists("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter.dll"));
        }


        [Test]
        void File_Exists_NoExtension()
        {
            Assert.IsTrue(_directory.FileExists("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter"));
        }

        [Test]
        void File_Exists_Extension_PrecedingSlash()
        {
            Assert.IsTrue(_directory.FileExists("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter.dll"));
        }

        [Test]
        void File_Exists_NoExtension_PrecedingSlash()
        {
            Assert.IsTrue(_directory.FileExists("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter"));
        }

        [Test]
        void Name_Of_Dll()
        {
            Assert.IsTrue(_directory.File("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter").Name ==
                          "BuildYourOwnSpaceCenter");
        }

        [Test]
        void Name_Of_Dll_WithPrecedingSlash()
        {
            Assert.IsTrue(_directory.File("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter").Name ==
              "BuildYourOwnSpaceCenter");
        }

        [Test]
        void FileName_Of_Dll()
        {
            Assert.IsTrue(_directory.File("BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter").FileName ==
                          "BuildYourOwnSpaceCenter.dll");
        }

        [Test]
        void FileName_Of_Dll_WithPrecedingSlash()
        {
            Assert.IsTrue(_directory.File("/BuildYourOwnSpaceCenter/BuildYourOwnSpaceCenter").FileName ==
              "BuildYourOwnSpaceCenter.dll");
        }
    }
}
