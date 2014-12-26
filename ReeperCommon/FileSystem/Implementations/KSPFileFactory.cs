using System;
using ReeperCommon.Extensions;

namespace ReeperCommon.FileSystem.Implementations
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
