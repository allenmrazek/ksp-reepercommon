using System;
using System.Collections.Generic;
using System.IO;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using System.Linq;

namespace ReeperCommon.Locators.Resources.Implementations
{
    public class ResourceRelativeToDirectory : IResourceLocator
    {
        private readonly IDirectory _directory;

        public ResourceRelativeToDirectory(IDirectory directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            _directory = directory;
        }



        public Maybe<byte[]> FindResource(string identifier)
        {
            var possiblePath = Path.Combine(_directory.Path, identifier);

            if (!File.Exists(possiblePath)) return Maybe<byte[]>.None;

            return Maybe<byte[]>.With(File.ReadAllBytes(possiblePath));
        }

        public IEnumerable<string> GetPossibilities()
        {
            return _directory.Files().Select(f => f.FileName);
        }
    }
}
