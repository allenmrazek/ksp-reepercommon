﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.TestData
{
    class SimplePersistentObjectNative : IReeperPersistent
    {
        public void DuringSerialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            
        }

        public void DuringDeserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {

        }
    }
}
