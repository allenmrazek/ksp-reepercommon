using ReeperCommon.Containers;

namespace ReeperCommon.FileSystem
{
    public interface ITemporaryFileFactory
    {
        TemporaryFile Create();
    }
}
