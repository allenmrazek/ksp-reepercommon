using System;

namespace ReeperCommon.Serialization
{
    public class NoSerializerFoundException : Exception
    {
        public NoSerializerFoundException(Type type):base(string.Format("No serializer found for {0}", type.FullName))
        {
            
        }
    }
}
