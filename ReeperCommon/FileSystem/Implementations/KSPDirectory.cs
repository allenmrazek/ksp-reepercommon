using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Extensions.Object;
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




        public Maybe<IDirectory> Directory(IUrlIdentifier url)
        {
            if (url.Depth < 1) return Maybe<IDirectory>.None;

            var dir = _directory.Children.FirstOrDefault(d => d.Name == url[0]);
            if (dir.IsNull()) return Maybe<IDirectory>.None;

            var found = _fsFactory.GetDirectory(dir);

            //return url.Depth <= 1 ? Maybe<IDirectory>.With(found) : found.Directory(url.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2));
            return url.Depth <= 1 ? 
                Maybe<IDirectory>.With(found) : 
                found.Directory(new KSPUrlIdentifier(url.Parts.Skip(1).Aggregate((s1, s2) => s1 + "/" + s2)));
        }



        public IEnumerable<IDirectory> Directories()
        {
            return _directory.Children
                .Select(url => _fsFactory.GetDirectory(url));
        }



        public IEnumerable<IDirectory> RecursiveDirectories()
        {
            return _directory.Children
                .Select(child => _fsFactory.GetDirectory(child))
                .Union(
                    _directory.Children
                        .SelectMany(child => _fsFactory.GetDirectory(child).RecursiveDirectories()));
        }



        public Maybe<IDirectory> Parent
        {
            get { return _directory.Parent.IsNull() ? Maybe<IDirectory>.None : Maybe <IDirectory>.With(_fsFactory.GetDirectory(_directory.Parent)); }
        }



        public bool FileExists(IUrlIdentifier url)
        {
            return File(url).Any();
        }



        public bool DirectoryExists(IUrlIdentifier url)
        {
            return Directory(url).Any();
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

            var files = Files();

            var withExt = files.Where(f => f.Extension == sanitized);

            return withExt;
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




        public Maybe<IFile> File(IUrlIdentifier url)
        {
            var filename = System.IO.Path.GetFileName(url.Path);
            var dirPath = System.IO.Path.GetDirectoryName(url.Path);

            if (!string.IsNullOrEmpty(dirPath))
            {
                var owningDirectory = Directory(new KSPUrlIdentifier(dirPath));

                return !owningDirectory.Any() ? Maybe<IFile>.None : owningDirectory.Single().File(new KSPUrlIdentifier(filename));
            }

            var file = _directory.Files
                .FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(filename) &&
                                     (((System.IO.Path.HasExtension(filename) && System.IO.Path.GetExtension(filename) == ("." + f.Extension)))
                                      ||
                                      (!System.IO.Path.HasExtension(filename))));


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

        public string Name
        {
            get { return _directory.Name; }
        }
    }
}
