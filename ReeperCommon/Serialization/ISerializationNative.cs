namespace ReeperCommon.Serialization
{
    // This serializer is used for types that supply their own internal serialization routines
    // (implement IReeperPersistent, in other words)
    public interface ISerializationNative : ISerializer
    {

    }
}
