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
        private readonly UrlDir _directory;
        private readonly IDirectoryFactory _directoryFactory;
        private readonly IFileFactory _fileFactory;




        public KSPDirectory(
            IDirectoryFactory directoryFactory,
            IFileFactory fileFactory, 
            UrlDir root)
        {
            if (directoryFactory == null) throw new ArgumentNullException("directoryFactory");
            if (root.IsNull())
                throw new ArgumentNullException("root");

            if (fileFactory.IsNull())
                throw new ArgumentNullException("fileFactory");

            _directoryFactory = directoryFactory;
            _directory = root;
            _fileFactory = fileFactory;
        }




        public Maybe<IDirectory> Directory(string url)
        {
            var identifier = new KSPUrlIdentifier(url);

            if (identifier.Depth < 1) return Maybe<IDirectory>.None;

            var dir = _directory.children.FirstOrDefault(d => d.name == identifier[0]);
            if (dir.IsNull()) return Maybe<IDirectory>.None;

            var found = _directoryFactory.Create(dir);

            return identifier.Depth <= 1 ? Maybe<IDirectory>.With(found) : found.Directory(identifier.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2));

            //return identifier.Depth > 1 ? found.Directory(identifier.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2)) : found;
        }



        public IEnumerable<IDirectory> Directories()
        {
            return _directory.children
                .Select(url => _directoryFactory.Create(url));
        }

        public Maybe<IDirectory> Parent
        {
            get { return _directory.parent.IsNull() ? Maybe<IDirectory>.None : Maybe <IDirectory>.With(_directoryFactory.Create(_directory.parent)); }
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
                _directory.files.Select(
                url => _fileFactory.Create(
                    _directoryFactory.Create(url.parent), 
                    url));
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
                    .Select(urlf => _fileFactory.Create(
                        _directoryFactory.Create(urlf.parent),
                        urlf));
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

                return owningDirectory.IsNull() ? Maybe<IFile>.None : owningDirectory.Single().File(filename);
            }

            var file = _directory.files
                .FirstOrDefault(f => f.name == System.IO.Path.GetFileNameWithoutExtension(filename) &&
                                     ((System.IO.Path.HasExtension(filename) &&
                                            System.IO.Path.GetExtension(filename) == ("." + f.fileExtension)))
                                      ||
                                      (!System.IO.Path.HasExtension(filename)));


            return file.IsNull()
                ? Maybe<IFile>.None
                : Maybe<IFile>.With(_fileFactory.Create(
                    _directoryFactory.Create(file.root), file));
        }



        private string GetFileName(UrlDir.UrlFile file)
        {
            return string.IsNullOrEmpty(file.fileExtension) ? file.name : file.name + "." + file.fileExtension;
        }



        public string FullPath
        {
            get { return _directory.path; } // fully qualified path
        }

        public string Url { get { return _directory.url; } }
    }
}
