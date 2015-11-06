using System;
using UnityEngine;

namespace ReeperCommon.Extensions
{
    public static class TransformExtensions
    {
// ReSharper disable once UnusedMember.Global
        public static Transform AddChild(this Transform transform, string name = "")
        {
            if (transform == null) throw new ArgumentNullException("transform");

            if (string.IsNullOrEmpty(name))
                name = "Child of " + transform.name;

            var ch = new GameObject(name) { layer = transform.gameObject.layer };

            ch.transform.parent = transform;
            ch.transform.localPosition = Vector3.zero;

            return ch.transform;
        }
    }
}
