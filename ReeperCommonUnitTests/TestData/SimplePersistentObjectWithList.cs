using System.Collections.Generic;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.TestData
{
    public class SimplePersistentObjectWithList
    {
        [ReeperPersistent]
        public List<string> ListField = new List<string>
            {
                "Apple",
                "Orange",
                "Pear"
            };
    }
}
