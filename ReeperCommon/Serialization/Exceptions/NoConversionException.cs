using System;

namespace ReeperCommon.Serialization.Exceptions
{
    public class NoConversionException : Exception
    {
        public NoConversionException(Type from, Type to)
            : base("No conversion from " + from.FullName + " to " + to.FullName + " available")
        {
        }
    }
}
