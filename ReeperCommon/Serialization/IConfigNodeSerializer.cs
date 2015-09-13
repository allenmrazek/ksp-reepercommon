namespace ReeperCommon.Serialization
{
    public interface IConfigNodeSerializer
    {
        ConfigNode CreateConfigNodeFromObject(object target);

        void WriteObjectToConfigNode(ref object source, ConfigNode config);
        void LoadObjectFromConfigNode(ref object target, ConfigNode config);

        ISerializerSelector SerializerSelector { get; set; }
    }
}
