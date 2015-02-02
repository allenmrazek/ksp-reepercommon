using System.Collections.Generic;
using ReeperCommon.FileSystem;

namespace ReeperCommonUnitTests.FileSystem.Framework
{
    public interface IFakeDirectory
    {
        string Name { get; }
        List<string> Files { get; }
        IEnumerable<string> AllFiles { get; }
        List<IFakeDirectory> Directories { get; }

        IUrlDir Construct(IUrlDir parent);
    }
}
