namespace ReeperCommon.Serialization
{
    public interface IConfigNodeSerializer
    {
        void Deserialize(object target, ConfigNode config);
        void Serialize(object source, ConfigNode config);

        ISerializerSelector SerializerSelector { get; set; }
    }
}
