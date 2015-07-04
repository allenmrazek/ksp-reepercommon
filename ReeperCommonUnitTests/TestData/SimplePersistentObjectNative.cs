using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.TestData
{
    class SimplePersistentObjectNative : IReeperPersistent
    {
        public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            
        }

        public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {

        }
    }
}
