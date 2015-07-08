using System;

namespace ReeperCommon.Serialization.Exceptions
{
    public class NoDefaultValueException : Exception
    {
        public NoDefaultValueException(Type type) : base("Could not create a default instance of " + type.FullName)
        {
        }
    }
}
