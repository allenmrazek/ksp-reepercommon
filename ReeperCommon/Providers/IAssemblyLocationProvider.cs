using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace ReeperCommon.Providers
{
    public interface IAssemblyLocationProvider
    {
        Maybe<IDirectory> GetDirectory(Assembly target);
        Maybe<IFile> Get(Assembly target);
    }
}
