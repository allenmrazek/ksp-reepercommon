namespace ReeperCommon.Serialization
{
    public interface IReeperPersistent
    {
        void Serialize(IConfigNodeSerializer formatter, ConfigNode node);
        void Deserialize(IConfigNodeSerializer formatter, ConfigNode node);
    }
}
