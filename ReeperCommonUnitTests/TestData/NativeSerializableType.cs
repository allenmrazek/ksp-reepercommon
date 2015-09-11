using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.TestData
{
    public class NativeSerializableType : IReeperPersistent
    {
        public void DuringSerialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            
        }

        public void DuringDeserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {

        }
    }
}