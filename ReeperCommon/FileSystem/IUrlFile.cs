using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem
{
    public interface IUrlFile
    {
        string FullPath { get; }
        string Extension { get; }
        string Name { get; }
        string Url { get; }
    }
}
