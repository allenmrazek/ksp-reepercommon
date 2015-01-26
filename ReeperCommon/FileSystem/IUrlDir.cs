using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem
{
    public interface IUrlDir
    {
        string Name { get; }
        string FullPath { get; }
        string Url { get; }
        IUrlDir Parent { get; }


        IEnumerable<IUrlDir> Children { get; }
        IEnumerable<IFile> Files { get; }
        IEnumerable<IFile> AllFiles { get; }
    }
}
