namespace ReeperCommon.Serialization
{
    // This serializer is for type that don't know how they're serialized and instead a 
    // second type (the surrogateSerializer) should do it for them
    public interface ISurrogateSerializer : IConfigNodeItemSerializer
    {
    }

    public interface ISurrogateSerializer<T> : ISurrogateSerializer
    {
        
    }
}
