using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.Logging;

namespace ReeperCommon.FileSystem.Implementations
{
// ReSharper disable once InconsistentNaming
    public class KSPDirectory : IDirectory
    {
        private readonly IFileSystemFactory _fsFactory;
        private readonly IUrlDir _directory;




        public KSPDirectory(
            IFileSystemFactory fsFactory,
            IUrlDir root)
        {
            if (fsFactory == null) throw new ArgumentNullException("fsFactory");
            if (root.IsNull())
                throw new ArgumentNullException("root");

  
            _fsFactory = fsFactory;
            _directory = root;
        }




        public Maybe<IDirectory> Directory(string url)
        {
            var identifier = new KSPUrlIdentifier(url);

            if (identifier.Depth < 1) return Maybe<IDirectory>.None;

            var dir = _directory.Children.FirstOrDefault(d => d.Name == identifier[0]);
            if (dir.IsNull()) return Maybe<IDirectory>.None;

            var found = _fsFactory.GetDirectory(dir);

            return identifier.Depth <= 1 ? Maybe<IDirectory>.With(found) : found.Directory(identifier.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2));
        }



        public IEnumerable<IDirectory> Directories()
        {
            return _directory.Children
                .Select(url => _fsFactory.GetDirectory(url));
        }

        public Maybe<IDirectory> Parent
        {
            get { return _directory.Parent.IsNull() ? Maybe<IDirectory>.None : Maybe <IDirectory>.With(_fsFactory.GetDirectory(_directory.Parent)); }
        }

        public bool FileExists(string url)
        {
            return !File(url).IsNull();
        }



        public bool DirectoryExists(string url)
        {
            return !Directory(url).IsNull();
        }



        public IEnumerable<IFile> Files()
        {
            return
                _directory.Files
                .Select(url => _fsFactory.GetFile(this, url));
        }



        public IEnumerable<IFile> Files(string extension)
        {
            var sanitized = extension.TrimStart('.');

            return Files().Where(f => f.Extension == sanitized);
        }



        public IEnumerable<IFile> RecursiveFiles()
        {
            return
                _directory.AllFiles
               .Select(url => _fsFactory.GetFile(this, url));
        }



        public IEnumerable<IFile> RecursiveFiles(string extension)
        {
            var sanitized = extension.TrimStart('.');

            return RecursiveFiles().Where(f => f.Extension == sanitized);
        }




        public Maybe<IFile> File(string url)
        {
            if (string.IsNullOrEmpty(url)) return Maybe<IFile>.None;

            var filename = System.IO.Path.GetFileName(url);
            var dirPath = System.IO.Path.GetDirectoryName(url);

            if (!string.IsNullOrEmpty(dirPath))
            {
                var owningDirectory = Directory(dirPath);

                return !owningDirectory.Any() ? Maybe<IFile>.None : owningDirectory.Single().File(filename);
            }

            var file = _directory.Files
                .FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(filename) &&
                                     ((System.IO.Path.HasExtension(filename) &&
                                            System.IO.Path.GetExtension(filename) == ("." + f.Extension)))
                                      ||
                                      (!System.IO.Path.HasExtension(filename)));


            return file.IsNull()
                ? Maybe<IFile>.None
                : Maybe<IFile>.With(_fsFactory.GetFile(this, file));
        }



        public string FullPath
        {
            get { return _directory.FullPath; } // fully qualified path
        }

        public string Url { get { return _directory.Url; } }
        public IUrlDir UrlDir { get { return _directory; }}
    }
}
