using System;

namespace ReeperCommon.Serialization.Exceptions
{
    public class AmbiguousKeyException : Exception
    {
        public AmbiguousKeyException(string key) : base("More than one match for key \"" + key + "\" found")
        {
        }
    }
}
