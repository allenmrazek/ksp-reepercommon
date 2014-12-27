using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Extensions;

namespace ReeperCommon.FileSystem.Implementations
{
// ReSharper disable once InconsistentNaming
    public class KSPDirectory : IDirectory
    {
        // because the stock version doesn't handle filenames with multiple periods well
        // which is a problem for us when we want to let people know a mu is a mu but also
        // hide it from the stock loader by naming it model.mu.structure (or whatever)
        private class Identifier
        {
            public readonly string _url;
            public readonly string[] _parts;

            public Identifier(string url)
            {
                url = url.Trim('\\', '/');

                _url = url;
                _parts = url.Split('/', '\\');
            }

            public string this[int i] 
            {
                get { return _parts[i]; }
            }

            public int Depth { get { return _parts.Length;  }}
        }


        private readonly UrlDir _directory;
        private readonly IFileFactory _fileFactory;



        public KSPDirectory(IFileFactory fileFactory, IGameDataPathQuery gameDataPath)
        {
            if (GameDatabase.Instance.IsNull())
                throw new InvalidOperationException("GameDatabase.Instance");

            if (fileFactory.IsNull())
                throw new ArgumentNullException("fileFactory");

            if (gameDataPath.IsNull())
                throw new ArgumentNullException("gameDataPath");

            _fileFactory = fileFactory;

            // the GameData directory isn't uniquely named among root's children
            // so we must examine paths to locate it
            _directory = gameDataPath.Directory();

            if (_directory.IsNull())
                throw new FieldAccessException("_gameData");
        }



        public KSPDirectory(IFileFactory fileFactory, UrlDir root)
        {
            if (root.IsNull())
                throw new ArgumentNullException("root");

            if (fileFactory.IsNull())
                throw new ArgumentNullException("fileFactory");

            _directory = root;
            _fileFactory = fileFactory;
        }



        public IDirectory Directory(string url)
        {
            return Directory(url, 0);
        }



        public IEnumerable<IDirectory> Directories()
        {
            return _directory.children
                .Select(url => new KSPDirectory(_fileFactory, url))
                .Cast<IDirectory>();
        }

        public IDirectory Parent
        {
            get { return new KSPDirectory(_fileFactory, _directory.parent); }
        }

        public bool FileExists(string url)
        {
            return true;
            return !File(url).IsNull();
        }



        public bool DirectoryExists(string url)
        {
            return !Directory(url).IsNull();
        }



        private IDirectory Directory(string url, int depth)
        {
            return string.IsNullOrEmpty(url) ? this : Directory(new Identifier(url), 0);
        }



        private IDirectory Directory(Identifier id, int depth)
        {
            if (depth == id.Depth - 1)
            {
                var dir = _directory.children.FirstOrDefault(u => u.name == id[depth]);
                return dir.IsNull() ? null : new KSPDirectory(_fileFactory, dir);
            }
            else
            {
                var childDir = _directory.children.FirstOrDefault(u => u.name == id[depth]);
                return childDir.IsNull() ? null : new KSPDirectory(_fileFactory, childDir).Directory(id, depth + 1);
            }
        }



        public IEnumerable<IFile> Files()
        {
            return
                _directory.files.Select(
                    url => _fileFactory.Create(new KSPDirectory(_fileFactory, url.parent), url));
        }



        public IEnumerable<IFile> Files(string extension)
        {
            return Files().Where(f => f.Extension == extension);
        }



        public IEnumerable<IFile> RecursiveFiles()
        {
            return
                _directory.AllFiles
                    .Select(urlf => _fileFactory.Create(
                        new KSPDirectory(_fileFactory, urlf.parent),
                        urlf));
        }



        public IEnumerable<IFile> RecursiveFiles(string extension)
        {
            return RecursiveFiles().Where(f => f.Extension == extension);
        }



        public IFile File(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            var found = FindFile(new Identifier(url), 0);

            return found.IsNull() ? null : _fileFactory.Create(new KSPDirectory(_fileFactory, found.parent), found);
        }



        private UrlDir.UrlFile FindFile(Identifier id, int depth)
        {
            if (id.Depth - 1 == depth)
            {
                // support KSP-style names with no extension and files with extension
                var urlFile = _directory.files.FirstOrDefault(f => f.name == id[depth]) ??
                _directory.files.FirstOrDefault(f => f.name == System.IO.Path.GetFileNameWithoutExtension(id[depth]) &&
                        "." + f.fileExtension == System.IO.Path.GetExtension(id[depth]));

                return urlFile;
            }
            else
            {
                var subdirectory = Directory(id[depth]) as KSPDirectory;
                return subdirectory.IsNull() ? null : subdirectory.FindFile(id, depth + 1);
            }
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
