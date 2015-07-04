namespace ReeperCommon.Serialization
{
    // This serializer is for type that don't know how they're serialized and instead a 
    // second type (the surrogate) should do it for them
    public interface ISerializationSurrogate : ISerializer
    {
    }

    public interface ISerializationSurrogate<T> : ISerializationSurrogate
    {
        
    }
}
