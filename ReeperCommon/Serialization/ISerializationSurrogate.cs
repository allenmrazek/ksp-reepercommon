namespace ReeperCommon.Serialization
{
    public interface ISerializationSurrogate
    {
        bool Serialize(object target, ConfigNode config, string valueName);
        bool Deserialize(object target, ConfigNode config, string valueName);
    }
}
