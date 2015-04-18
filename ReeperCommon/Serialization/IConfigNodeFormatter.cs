namespace ReeperCommon.Serialization
{
    public interface IConfigNodeFormatter
    {
        bool Deserialize(object target, ConfigNode config);
        bool Serialize(object source, ConfigNode config);

        ISurrogateSelector SurrogateSelector { get; set; }
    }
}
