using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.GameLoadState.Attributes;

namespace ReeperCommon.GameLoadState.Providers
{
    class KSPAssemblyMarkedTypeProvider : ILoadStateMarkedTypeProvider
    {
        public IEnumerable<Type> GetMarkedTypesFor(LoadStateMarker.State state)
        {
            return AssemblyLoader.loadedAssemblies.SelectMany(la => ForState(GetMarkedTypes(la.assembly), state));
        }

        private IEnumerable<Type> GetMarkedTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(ty => ty.GetCustomAttributes(typeof(LoadStateMarker), true).Length > 0 &&
                                                   ty.GetCustomAttributes(typeof(KSPAddon), true).Length > 0);
        }

        private IEnumerable<Type> ForState(IEnumerable<Type> types, LoadStateMarker.State state)
        {
            return
                types.Where(
                    ty =>
                        ty.GetCustomAttributes(typeof (LoadStateMarker), true)
                            .Any(st => ((LoadStateMarker.State) st) == state));

        }
    }
}
