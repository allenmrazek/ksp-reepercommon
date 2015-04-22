namespace ReeperCommon.Serialization
{
    public interface IReeperPersistent
    {
        void Save(IConfigNodeFormatter formatter, ConfigNode node);
        void Load(IConfigNodeFormatter formatter, ConfigNode node);
    }
}
