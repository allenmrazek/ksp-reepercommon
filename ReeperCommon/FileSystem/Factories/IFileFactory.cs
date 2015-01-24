namespace ReeperCommon.FileSystem.Factories
{
    public interface IFileFactory
    {
        IFile Create(IDirectory directory, UrlDir.UrlFile file);
    }
}
