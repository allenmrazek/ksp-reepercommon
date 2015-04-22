using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Serialization;
using UnityEngine;

namespace ReeperCommonUnitTests.TestData
{
    public class ComplexPersistentObject
    {
        public class InternalPersistent : IReeperPersistent, IPersistenceLoad, IPersistenceSave
        {
            public void Save(IConfigNodeFormatter formatter, ConfigNode node)
            {
                node.AddValue("InternalPersistent", "TestValue");
            }

            public void Load(IConfigNodeFormatter formatter, ConfigNode node)
            {
                if (!node.HasValue("InternalPersistent")) throw new Exception("No value named InternalPersistent");
            }

            public void PersistenceLoad()
            {
                
            }

            public void PersistenceSave()
            {

            }
        }


        [ReeperPersistent] public IReeperPersistent MyTestObject = new InternalPersistent();
        [ReeperPersistent] public string MyTestString = "Hello, world";
        [ReeperPersistent] public int MyIntegerValue = 123;
        [ReeperPersistent] public float MyFloatValue = 123.45f;
        [ReeperPersistent] public double MyDoubleValue = 123.45678f;
        [ReeperPersistent] public Vector2 MyTestVector2 = new Vector2(12f, 21f);
    }
}
