using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Serialization;

namespace ReeperCommonUnitTests.Serialization.Complex
{
    public class NativeSerializableObjectWithNativeSerializableField : IReeperPersistent
    {
        public class SubObject : IReeperPersistent
        {
            public void DuringSerialize(IConfigNodeSerializer serializer, ConfigNode node)
            {
                node.AddValue("From", GetType().Name);
            }

            public void DuringDeserialize(IConfigNodeSerializer serializer, ConfigNode node)
            {

            }
        }

        [ReeperPersistent] private SubObject SubField = new SubObject();

        public void DuringSerialize(IConfigNodeSerializer serializer, ConfigNode node)
        {
            node.AddValue("From", GetType().Name);
        }

        public void DuringDeserialize(IConfigNodeSerializer serializer, ConfigNode node)
        {

        }
    }
}
