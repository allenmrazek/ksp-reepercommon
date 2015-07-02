using System;

namespace ReeperCommon.FileSystem
{
// ReSharper disable once InconsistentNaming
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
            if (directory == null) throw new ArgumentNullException("directory");
            if (file == null) throw new ArgumentNullException("file");

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
