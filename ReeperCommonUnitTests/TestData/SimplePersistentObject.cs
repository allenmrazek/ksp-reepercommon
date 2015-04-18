using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommonUnitTests.TestData
{
    public class SimplePersistentObject
    {
        [Persistent]
        public string PersistentField = "Value";

        [NonSerialized]
        public string NonpersistentField = "Anonymous";
    }
}
