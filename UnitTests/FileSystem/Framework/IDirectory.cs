using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem;

namespace UnitTests.FileSystem.Framework
{
    interface IDirectory
    {
        string Name { get; }
        List<string> Files { get; }
        IEnumerable<string> AllFiles { get; }
        List<IDirectory> Directories { get; }

        IUrlDir Build();

    }
}
