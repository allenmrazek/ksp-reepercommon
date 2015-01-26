using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem.Factories
{
    public interface IFileSystemFactory
    {
        IFile GetFile(IDirectory directory, IUrlFile file);
        IDirectory GetDirectory(IUrlDir dir);


        IDirectory GetGameDataDirectory();
    }
}
