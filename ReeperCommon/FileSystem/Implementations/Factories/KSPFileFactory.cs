using System;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem.Factories;

namespace ReeperCommon.FileSystem.Implementations.Factories
{
    public class KSPFileFactory : IFileFactory
    {
        public IFile Create(IDirectory directory, UrlDir.UrlFile file)
        {
            if (directory.IsNull())
                throw new ArgumentNullException("directory");

            return new KSPFile(directory, file);
        }
    }
}
