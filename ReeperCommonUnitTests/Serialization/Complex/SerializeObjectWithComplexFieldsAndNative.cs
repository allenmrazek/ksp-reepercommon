using System.Collections.Generic;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.Serialization.Complex
{
    public class SerializeObjectWithComplexFieldsAndNative : IReeperPersistent
    {
        [ReeperPersistent] public string HelloWorldField = "Hello, world!";
        [ReeperPersistent] public ConfigNode SimpleConfigNodeField = new ConfigNode("Simple");
        [ReeperPersistent] public List<float> FloatListField = new List<float> {1f, 2f, 3f};
        [ReeperPersistent] public List<string> StringListField = new List<string> {"apple", "banana", "pear"};
        [ReeperPersistent] public List<ConfigNode> ConfigNodeListField = new List<ConfigNode>
        {
            new ConfigNode("first"),
            new ConfigNode("second"),
            new ConfigNode("third")
        }; 

        public void DuringSerialize(IConfigNodeSerializer serializer, ConfigNode node)
        {
            node.AddValue("complexName", "complexValue");
        }

        public void DuringDeserialize(IConfigNodeSerializer serializer, ConfigNode node)
        {

        }
    }
}
