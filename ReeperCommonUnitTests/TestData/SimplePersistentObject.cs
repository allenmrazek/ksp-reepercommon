using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.TestData
{
    public class SimplePersistentObject
    {
        [ReeperPersistent]
        public string PersistentField = "Value";

        public string NonpersistentField = "Anonymous";
    }
}
