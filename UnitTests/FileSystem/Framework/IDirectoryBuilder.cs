using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.FileSystem.Framework
{
    public interface IDirectoryBuilder
    {
        ReeperCommon.FileSystem.IDirectory Build();

        IDirectoryBuilder WithDirectory(string name);
        IDirectoryBuilder WithFile(string filename);
        IDirectoryBuilder MakeDirectory(string name);
        IDirectoryBuilder Parent();
    }
}
