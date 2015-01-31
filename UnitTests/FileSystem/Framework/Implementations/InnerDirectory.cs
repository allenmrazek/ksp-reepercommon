using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem;

namespace UnitTests.FileSystem.Framework.Implementations
{
    class InnerDirectory : Directory
    {
        private readonly IDirectoryBuilder _builder;

        public InnerDirectory(string name, IDirectoryBuilder builder, IUrlFileMocker fmocker) : base(name, fmocker)
        {
            if (builder == null) throw new ArgumentNullException("builder");
            _builder = builder;
        }


        public override IUrlDir Build()
        {
            return _builder.Build().UrlDir;
        }
    }
}
