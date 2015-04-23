using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace ReeperCommon.Providers
{
    interface IAssemblyLocationProvider
    {
        Maybe<IDirectory> Get(Assembly target);
    }
}
