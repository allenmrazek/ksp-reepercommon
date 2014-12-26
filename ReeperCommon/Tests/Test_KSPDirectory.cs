using System.IO;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace ReeperCommon.Tests
{
    [TestSuite]
    class Test_KSPDirectory
    {
        private IDirectory _directory;



        [TestSetup]
        void DoSetup()
        {
            _directory = new KSPDirectory(new KSPFileFactory());
        }



        [Test]
        void DefaultConstructed_Is_GameData()
        {
            Assert.IsTrue(_directory.Path.EndsWith("GameData"));
        }


        [Test]
        void DefaultConstructed_Path_IsGameData()
        {
            var kspGameData = KSPUtil.ApplicationRootPath + "/GameData";

            Assert.IsTrue(Path.GetFullPath(_directory.Path) == Path.GetFullPath(kspGameData));
        }



        [Test]
        void Found_First_Subdir_Of_BYOSC()
        {
            Assert.IsTrue(_directory.Directory("BuildYourOwnSpaceCenter") != null);
        }



        

        [Test]
        void PluginDirectory_Exists()
        {
            Assert.IsTrue(_directory.DirectoryExists("BuildYourOwnSpaceCenter"));
        }

        [Test]
        void PluginDirectory_Exists_PrecedingSlash()
        {
            Assert.IsTrue(_directory.DirectoryExists("/BuildYourOwnSpaceCenter"));
        }

        [Test]
        void PluginDirectory_Subdir_Exists()
        {
            Assert.IsTrue(_directory.DirectoryExists("BuildYourOwnSpaceCenter/structures"));
        }

        [Test]
        void PluginDirectory_Subdir_Exists_PrecedingSlash()
        {
            Assert.IsTrue(_directory.DirectoryExists("/BuildYourOwnSpaceCenter/structures"));
        }


        [Test]
        void Subdirectory_Lists_All_Files()
        {
            string path = _directory.Path + "/BuildYourOwnSpaceCenter";
            var files = System.IO.Directory.GetFiles(path).Select(s => System.IO.Path.GetFileName(s));

            Assert.IsEqualSequence(files.OrderBy(s => s), Enumerable.OrderBy<string, string>(_directory.Directory("BuildYourOwnSpaceCenter").Files().Select(f => f.FileName), s => s));
        }
    }
}
