using System;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Extensions.GameObject
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
    }
}
