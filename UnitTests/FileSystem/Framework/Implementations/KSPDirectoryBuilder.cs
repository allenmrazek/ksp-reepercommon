using System;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using ReeperCommon.Extensions.Object;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using System.Linq;

namespace UnitTests.FileSystem.Framework.Implementations
{
    public class KSPDirectoryBuilder : IDirectoryBuilder
    {
        private readonly IUrlFileMocker _fmocker;
        private readonly IDirectoryBuilder _parent;
        private readonly IDirectory _thisDirectory;
        private readonly List<Action<KSPDirectory>> _actions = new List<Action<KSPDirectory>>();


       

        public KSPDirectoryBuilder(string name, IUrlFileMocker fmocker)
        {
            if (fmocker == null) throw new ArgumentNullException("fmocker");
            
            if (name == null || string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            _fmocker = fmocker;
            _thisDirectory = new Directory(name, fmocker);
        }



        public KSPDirectoryBuilder(string name, IDirectoryBuilder parent, IUrlFileMocker fmocker):this(name, fmocker)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            _parent = parent;
        }



        public ReeperCommon.FileSystem.IDirectory Build()
        {
            var root = _thisDirectory.Build();

            var fss = new KSPFileSystemFactory(root);
            var dir = new KSPDirectory(fss, root);

            _actions.ForEach(action => action(dir));

            return dir;
        }



        public ReeperCommon.FileSystem.IDirectory BuildAll()
        {
            return !_parent.IsNull() ? _parent.BuildAll() : Build();
        }



        public IDirectoryBuilder WithDirectory(string name)
        {
            if (_thisDirectory.Directories.Any(d => d.Name == name))
                throw new InvalidOperationException(_thisDirectory.Name + " already contains directory called " + name);

            _thisDirectory.Directories.Add(new Directory(name, _fmocker));
            return this;
        }



        public IDirectoryBuilder WithFile(string filename)
        {
            if (_thisDirectory.Files.Any(f => f == filename))
                throw new InvalidOperationException(_thisDirectory.Name + " already contains file " + filename);

            _thisDirectory.Files.Add(filename);

            return this;
        }



        public IDirectoryBuilder MakeDirectory(string name)
        {
            if (_thisDirectory.Directories.Any(d => d.Name == name))
                throw new InvalidOperationException(_thisDirectory.Name + " already contains directory called " + name);

            var newBuilder = new KSPDirectoryBuilder(name, this, _fmocker);

            _thisDirectory.Directories.Add(new InnerDirectory(name, newBuilder, _fmocker));

            return newBuilder;
        }



        public IDirectoryBuilder Parent()
        {
            if (_parent.IsNull())
                throw new InvalidOperationException(_thisDirectory.Name + " does not have a parent directory");

            return _parent;
        }
    }
}
