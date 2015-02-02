using System.Collections.Generic;

namespace ReeperCommonUnitTests.FileSystem.Framework
{
    public interface IFakeDirectoryBuilder
    {
        ReeperCommon.FileSystem.IDirectory Build();

        IFakeDirectoryBuilder WithDirectory(string name);
        IFakeDirectoryBuilder WithFile(string filename);
        IFakeDirectoryBuilder MakeDirectory(string name);
        IFakeDirectoryBuilder Parent();

        IEnumerable<IFakeDirectory> Directories { get; }
        //IFakeDirectory FakeDirectory { get; }
    }
}
