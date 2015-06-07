using System;

namespace ReeperCommon.Serialization
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ReeperPersistentAttribute : Attribute
    {
    }
}
