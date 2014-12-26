namespace ReeperCommon.FileSystem
{
    public interface IFileFactory
    {
        IFile Create(IDirectory directory, UrlDir.UrlFile file);
    }
}
