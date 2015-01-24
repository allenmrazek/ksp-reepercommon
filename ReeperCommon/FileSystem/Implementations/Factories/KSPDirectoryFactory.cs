using System;
using ReeperCommon.FileSystem.Factories;

namespace ReeperCommon.FileSystem.Implementations.Factories
{
    public class KSPDirectoryFactory : IDirectoryFactory
    {
        private readonly IUrlDirProvider _gameDataRoot;
        private readonly IFileFactory _fileFactory;

        public KSPDirectoryFactory(
            IFileFactory fileFactory,
            IUrlDirProvider gameDataRoot)
        {
            if (gameDataRoot == null) throw new ArgumentNullException("gameDataRoot");
            if (fileFactory == null) throw new ArgumentNullException("fileFactory");

            _gameDataRoot = gameDataRoot;
            _fileFactory = fileFactory;
        }



        public IDirectory Create(UrlDir parent)
        {
            return new KSPDirectory(this, _fileFactory, parent);
        }

        public IDirectory GetGameDataDir()
        {
            return new KSPDirectory(this, _fileFactory, _gameDataRoot.Directory());
        }
    }
}
