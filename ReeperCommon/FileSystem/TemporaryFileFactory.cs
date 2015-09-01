using System;
using System.IO;

namespace ReeperCommon.FileSystem
{
// ReSharper disable once UnusedMember.Global
    public class TemporaryFileFactory : ITemporaryFileFactory
    {
        public TemporaryFile Create()
        {
            return new TemporaryFile(Path.GetTempFileName());
        }

        public TemporaryFile Create(string fileNameInTempDir)
        {
            if (string.IsNullOrEmpty(fileNameInTempDir)) throw new ArgumentException("fileNameInTempDir");

            var sanitized = fileNameInTempDir.TrimStart('/', '\\').TrimEnd('/', '\\');

            if (sanitized.Length == 0)
                throw new ArgumentException("Bad filename: " + fileNameInTempDir, "fileNameInTempDir");

            var fullPath = Path.GetTempPath() + Path.DirectorySeparatorChar + sanitized;

            using (var f = File.Create(fullPath))
                return new TemporaryFile(fullPath);
        }
    }
}
