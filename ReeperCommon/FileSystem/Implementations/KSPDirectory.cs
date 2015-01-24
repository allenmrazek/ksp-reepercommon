using System;
using System.Collections.Generic;
using System.Linq;
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




        public IDirectory Directory(string url)
        {
            var identifier = new KSPUrlIdentifier(url);

            if (identifier.Depth < 1) return null;

            var dir = _directory.children.FirstOrDefault(d => d.name == identifier[0]);
            if (dir.IsNull()) return null;

            var found = _directoryFactory.Create(dir);

            return identifier.Depth > 1 ? found.Directory(identifier.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2)) : found;
        }



        public IEnumerable<IDirectory> Directories()
        {
            return _directory.children
                .Select(url => _directoryFactory.Create(url));
        }

        public IDirectory Parent
        {
            get { return _directoryFactory.Create(_directory.parent); }
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



        public IFile File(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            var filename = System.IO.Path.GetFileName(url);
            var dirPath = System.IO.Path.GetDirectoryName(url);

            if (!string.IsNullOrEmpty(dirPath) && DirectoryExists(dirPath))
                return Directory(dirPath).File(filename);

            var file = _directory.files
                .FirstOrDefault(f => f.name == System.IO.Path.GetFileNameWithoutExtension(filename) &&
                                     ((System.IO.Path.HasExtension(filename) &&
                                            System.IO.Path.GetExtension(filename) == ("." + f.fileExtension)))
                                      ||
                                      (!System.IO.Path.HasExtension(filename)));

            return file.IsNull()
                ? null
                : _fileFactory.Create(
                    _directoryFactory.Create(file.root), file);
        }



        public IFile File(string url, ILog log)
        {
            if (string.IsNullOrEmpty(url)) return null;

            log.Normal("File with " + url);

            var filename = System.IO.Path.GetFileName(url);
            var dirPath = System.IO.Path.GetDirectoryName(url);

            log.Normal("filename = " + filename);
            log.Normal("dirpath = " + dirPath);

            if (!string.IsNullOrEmpty(dirPath))// && DirectoryExists(dirPath))
            {
                log.Normal("looking for " + dirPath);

                if (!DirectoryExists(dirPath)) log.Warning("direxists = false on " + dirPath);

                var result = Directory(dirPath);

                if (result.IsNull()) log.Error("didn't find dir");

                    var fileresult = result.File(filename);

                    if (fileresult.IsNull()) log.Error("didnt find file result");
                else log.Normal("found dir");

                    return fileresult;
            }

            var file = _directory.files
                .FirstOrDefault(f => f.name == System.IO.Path.GetFileNameWithoutExtension(filename) &&
                                     ((System.IO.Path.HasExtension(filename) &&
                                            System.IO.Path.GetExtension(filename) == ("." + f.fileExtension)))
                                      ||
                                      (!System.IO.Path.HasExtension(filename)));

            if (file.IsNull()) log.Error("didn't find " + url);

            return file.IsNull()
                ? null
                : _fileFactory.Create(
                    _directoryFactory.Create(file.root), file);
        }



        private string GetFileName(UrlDir.UrlFile file)
        {
            return string.IsNullOrEmpty(file.fileExtension) ? file.name : file.name + "." + file.fileExtension;
        }



        public string Path
        {
            get { return _directory.path; }
        }
    }
}
