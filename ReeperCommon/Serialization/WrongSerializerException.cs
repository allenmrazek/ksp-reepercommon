using System;

namespace ReeperCommon.Serialization
{
    // don't expect to see this unless I've screwed up somewhere with the serializer selector
    public class WrongSerializerException : Exception
    {
        public WrongSerializerException(Type objectType, Type expected)
            : base("This serializer is for " + expected.FullName + "; received " + objectType.FullName)
        {
        }
    }
}
