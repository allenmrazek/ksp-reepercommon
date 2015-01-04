using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.GameLoadState.Attributes;
using UnityEngine;

namespace ReeperCommon.GameLoadState
{
    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class LoadStateMarkedTypeCreator : MonoBehaviour
    {
        void Start()
        {
            gameObject.AddComponent<LoadState_Immediate>();
            gameObject.AddComponent<LoadState_FileTreeReady>();
            gameObject.AddComponent<LoadState_TexturesLoaded>();
        }


        public IEnumerable<Type> GetMarkedTypesFor(LoadStateMarker.State state)
        {
            return AssemblyLoader.loadedAssemblies.SelectMany(la => ForState(GetMarkedTypes(la.assembly), state));
        }

        private IEnumerable<Type> GetMarkedTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(ty => ty.GetCustomAttributes(typeof(LoadStateMarker), true).Length > 0 &&
                                                   ty.IsAssignableFrom(typeof(MonoBehaviour)));
        }

        private IEnumerable<Type> ForState(IEnumerable<Type> types, LoadStateMarker.State state)
        {
            return
                types.Where(
                    ty =>
                        ty.GetCustomAttributes(typeof(LoadStateMarker), true)
                            .Any(st => ((LoadStateMarker.State)st) == state));

        }


        public void CreateTypesFor(LoadStateMarker.State state)
        {
            print("Creating types for: " + state.ToString());

            foreach (var ty in GetMarkedTypesFor(state))
            {
                var go = new GameObject("LoadState." + state + "." + ty.Name);
                go.AddComponent(ty);
            }

            print("Done creating types");
        }
    }
}
