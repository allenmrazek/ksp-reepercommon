using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace ReeperCommon.Providers
{
    public class AssemblyLocationProvider : IAssemblyLocationProvider
    {
        private readonly IFileSystemFactory _fsFactory;

        public AssemblyLocationProvider(IFileSystemFactory fsFactory)
        {
            if (fsFactory == null) throw new ArgumentNullException("fsFactory");
            _fsFactory = fsFactory;
        }


        public Maybe<IDirectory> Get(Assembly target)
        {
            var laLocation = AssemblyLoader.loadedAssemblies.FirstOrDefault(la => ReferenceEquals(la.assembly, target));

            return laLocation == null ? Maybe<IDirectory>.None : _fsFactory.GetGameDataDirectory().Directory(new KSPUrlIdentifier(laLocation.url));
        }
    }
}
