using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReeperCommon.Extensions
{
    public static class GameObjectExtensions
    {
        public delegate void VisitorDelegate(GameObject go, int depth);


        public static void TraverseHierarchy(this GameObject go, VisitorDelegate visitor)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (visitor == null) throw new ArgumentNullException("visitor");
            TraverseHierarchy(go, visitor, 0);
        }

        private static void TraverseHierarchy(GameObject go, VisitorDelegate visitor, int depth)
        {
            visitor(go, depth);

            foreach (Transform t in go.transform)
                TraverseHierarchy(t.gameObject, visitor, depth + 1);
        }


        public static void PrintComponents(this GameObject go, ILog baseLog)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (baseLog == null) throw new ArgumentNullException("baseLog");

            go.TraverseHierarchy((gameObject, depth) =>
            {
                PrintComponentsOf(gameObject, depth, baseLog);
            });
        }


        private static void PrintComponentsOf([NotNull] this GameObject go, int depth, [NotNull] ILog log)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (log == null) throw new ArgumentNullException("log");

            log.Debug("{0}{1} has components:", depth > 0 ? new string('-', depth) + ">" : "", go.name);

            var components = go.GetComponents<Component>();
            foreach (var c in components)
            {

                log.Debug("{0}: {1}", new string('.', depth + 3) + "c",
                    c == null ? "[missing script]" : c.GetType().FullName);
            }
        }


        public static void PrintAncestorHierarchy([NotNull] this GameObject go, [NotNull] ILog baseLog)
        {
            if (go == null) throw new ArgumentNullException("go");
            if (baseLog == null) throw new ArgumentNullException("baseLog");

            var stack = new Stack<GameObject>();
            var current = go.transform;
            int depth = 0;

            do
            {
                stack.Push(current.gameObject);
                current = current.transform.parent;
            } while (current != null);

            while (stack.Any())
            {
                var nextItem = stack.Pop();

                PrintComponentsOf(nextItem, depth, baseLog);
                depth++;
            }
        }


        public static void StripComponents<TComponent>(this GameObject go, bool recursive = true) where TComponent : Component
        {
            if (go == null) throw new ArgumentNullException("go");

            go.GetComponents<TComponent>()
                .ToList()
                .ForEach(Object.Destroy);

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
