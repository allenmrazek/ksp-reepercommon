using System;

namespace ReeperCommon.Serialization
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GameSerializedAttribute : Attribute
    {
    }
}
