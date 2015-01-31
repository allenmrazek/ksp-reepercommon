using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem;

namespace UnitTests.FileSystem.Framework
{
    public interface IUrlFileMocker
    {
        IUrlFile Get(string filename);
    }
}
