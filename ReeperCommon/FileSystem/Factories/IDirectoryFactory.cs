namespace ReeperCommon.FileSystem.Factories
{
    public interface IDirectoryFactory
    {
        IDirectory Create(UrlDir root);
        IDirectory GetGameDataDir();
    }
}
