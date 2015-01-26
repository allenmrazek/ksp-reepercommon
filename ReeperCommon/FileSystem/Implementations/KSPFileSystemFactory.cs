using System;
using ReeperCommon.FileSystem.Factories;

namespace ReeperCommon.FileSystem.Implementations
{
    public class KSPFileSystemFactory : IFileSystemFactory
    {
        private readonly IDirectory _gameData;

        public KSPFileSystemFactory(IUrlDir gameData)
        {
            if (gameData == null) throw new ArgumentNullException("gameData");
            _gameData = GetDirectory(gameData);
        }



        public IFile GetFile(IDirectory directory, IUrlFile file)
        {
            return new KSPFile(directory, file);
        }

        public IDirectory GetDirectory(IUrlDir dir)
        {
            return new KSPDirectory(this, dir);
        }

        public IDirectory GetGameDataDirectory()
        {
            return _gameData;
        }
    }
}
