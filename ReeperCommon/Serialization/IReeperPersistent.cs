namespace ReeperCommon.Serialization
{
    public interface IReeperPersistent
    {
        // Serialize bits that aren't handled via field surrogates here. DO NOT formatter.Serialize(this)!
        // That's nonsensical and will lead to stack overflows
        void DuringSerialize(IConfigNodeSerializer formatter, ConfigNode node);
        void DuringDeserialize(IConfigNodeSerializer formatter, ConfigNode node);
    }
}
