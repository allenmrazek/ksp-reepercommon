using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem.Factories;

namespace ReeperCommon.FileSystem.Implementations
{
    class KSPUrlDir : IUrlDir
    {
        private readonly UrlDir _kspDir;
        private readonly IDirectory _container;
        private readonly IFileSystemFactory _fsFactory;

        public KSPUrlDir(
            UrlDir kspDir,
            IDirectory container,
            IFileSystemFactory fsFactory)
        {
            if (kspDir == null) throw new ArgumentNullException("kspDir");
            if (container == null) throw new ArgumentNullException("container");
            if (fsFactory == null) throw new ArgumentNullException("fsFactory");
            _kspDir = kspDir;
            _container = container;
            _fsFactory = fsFactory;
        }


        public string Name { get { return _kspDir.name; }}
        public string FullPath {
            get { return _kspDir.path; }
        }
        public string Url { get { return _kspDir.url; } }
        public IUrlDir Parent {
            get { return new KSPUrlDir(_kspDir.parent, _container, _fsFactory); }
        }
        public IEnumerable<IUrlDir> Children { get { return _kspDir.children.Select(child => new KSPUrlDir(child, _container, _fsFactory)).Cast<IUrlDir>(); } }
        public IEnumerable<IFile> Files {
            get { return _kspDir.files.Select(f => _fsFactory.GetFile(_container, _fsFactory.GetUrlFile(f))); }
        }

        public IEnumerable<IFile> AllFiles
        {
            get { return _kspDir.AllFiles.Select(f => _fsFactory.GetFile(_container, _fsFactory.GetUrlFile(f))); }
        }
    }
}
