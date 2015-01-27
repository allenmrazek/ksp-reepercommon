using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem
{
    public interface IUrlFile
    {
        string FullPath { get; } // path on drive, including root
        string Extension { get; } // exclude period in extension
        string Name { get; } // filename, excluding extension
        string Url { get; } // path from GameData, excluding extension
    }
}
