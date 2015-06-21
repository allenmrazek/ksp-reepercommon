namespace ReeperCommon.Serialization
{
    public interface IConfigNodeSerializer
    {
        void Deserialize(object target, ConfigNode config);
        void Serialize(object source, ConfigNode config);

        ISurrogateSelector SurrogateSelector { get; set; }
    }
}
