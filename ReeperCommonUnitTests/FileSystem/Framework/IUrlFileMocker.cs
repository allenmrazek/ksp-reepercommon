using ReeperCommon.FileSystem;

namespace ReeperCommonUnitTests.FileSystem.Framework
{
    public interface IUrlFileMocker
    {
        IUrlFile Get(string filename);
    }
}
