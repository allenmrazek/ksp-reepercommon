using System;
using System.Linq;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Extensions
{
    public static class GameObjectExtensions
    {
        public delegate void VisitorDelegate(UnityEngine.GameObject go, int depth);


        public static void TraverseHierarchy(this UnityEngine.GameObject go, VisitorDelegate visitor)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (visitor == null) throw new ArgumentNullException("visitor");
            TraverseHierarchy(go, visitor, 0);
        }

        private static void TraverseHierarchy(UnityEngine.GameObject go, VisitorDelegate visitor, int depth)
        {
            visitor(go, depth);

            foreach (Transform t in go.transform)
                TraverseHierarchy(t.gameObject, visitor, depth + 1);
        }


        public static void PrintComponents(this UnityEngine.GameObject go, ILog baseLog)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (baseLog == null) throw new ArgumentNullException("baseLog");

            go.TraverseHierarchy((gameObject, depth) =>
            {
                baseLog.Debug("{0}{1} has components:", depth > 0 ? new string('-', depth) + ">" : "", gameObject.name);

                var components = gameObject.GetComponents<Component>();
                foreach (var c in components)
                    baseLog.Debug("{0}: {1}", new string('.', depth + 3) + "c", c.GetType().FullName);
            });
        }


        public static void StripComponents<TComponent>(this GameObject go, bool recursive = true) where TComponent : Component
        {
            if (go == null) throw new ArgumentNullException("go");

            go.GetComponents<TComponent>()
                .ToList()
                .ForEach(UnityEngine.Object.Destroy);

            if (!recursive) return;

            foreach (Transform ch in go.transform)
                StripComponents<TComponent>(ch.gameObject);
        }


        public static void StripComponents<T1, T2>(this GameObject go, bool recursive = true) 
            where T1 : Component 
            where T2 : Component
        {
            StripComponents<T1>(go, recursive);
            StripComponents<T2>(go, recursive);
        }


        public static void StripComponents<T1, T2, T3>(this GameObject go, bool recursive = true)
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            StripComponents<T1>(go, recursive);
            StripComponents<T2>(go, recursive);
            StripComponents<T3>(go, recursive);
        }

// ReSharper disable once UnusedMember.Global
        public static GameObject AddChild(this GameObject go, string name)
        {
            return go.transform.AddChild(name).gameObject;
        }
    }
}
