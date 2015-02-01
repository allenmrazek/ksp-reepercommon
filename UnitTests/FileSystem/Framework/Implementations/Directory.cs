using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ReeperCommon.FileSystem;

namespace UnitTests.FileSystem.Framework.Implementations
{
    class Directory : IDirectory
    {
        protected readonly List<string> _files = new List<string>();

        private readonly string _name;
        private readonly IUrlFileMocker _fmocker;



        public Directory(string name, IUrlFileMocker fmocker)
        {
            if (fmocker == null) throw new ArgumentNullException("fmocker");
            if (name == null || string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            name = name.Trim('/', '\\');

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
            _fmocker = fmocker;
            Directories = new List<IDirectory>();
        }



        public string Name
        {
            get { return _name; }
        }



        public IEnumerable<string> AllFiles
        {
            get
            {
                return Files.Union(
                                    Directories.SelectMany(dir => dir.AllFiles.Select(f => dir.Name + "/" + f))
                                  );
            }
        }



        public List<IDirectory> Directories { get; private set; }



        public List<string> Files
        {
            get
            {
                return _files;
            }
        }





        public virtual IUrlDir Build()
        {
            return Build(null);
        }



        public IUrlDir Build(IUrlDir parent)
        {
            var root = Substitute.For<IUrlDir>();

            root.Name.Returns(Name);
            root.FullPath.Returns("C:/" + Name + "/");
            root.Parent.Returns(parent);

            var mockedFiles = Files.Select(filename => _fmocker.Get(filename)); // uses NSubstitute, so don't put inside Returns()

            root.Files.Returns(mockedFiles); // files in same dir

            var allMockedFiles = AllFiles.Select(f => _fmocker.Get(f));

            root.AllFiles.Returns(allMockedFiles);

            //var children = Directories.Select(dir => dir.Build(root));
            var children = new List<IUrlDir>();

            foreach (var dir in Directories)
                children.Add(dir.Build(root));

            root.Children.Returns(children);

            foreach (var child in children)
                child.Parent.Returns(root);

            return root;
        }
    }
}
