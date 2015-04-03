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
            if (directory == null) throw new ArgumentNullException("directory");
            if (file == null) throw new ArgumentNullException("file");

            if (!directory.FileExists(new KSPUrlIdentifier(file.Name)))
                throw new Exception("Directory " + directory.Url + "(" + directory.FullPath + ")" + " does not contain file " + file.Url);

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
