using System;

namespace ReeperCommon.Serialization.Exceptions
{
    public class AmbiguousKeyException : ReeperSerializationException
    {
        public AmbiguousKeyException(string key) : base("More than one match for key \"" + key + "\" found")
        {
        }
    }
}
