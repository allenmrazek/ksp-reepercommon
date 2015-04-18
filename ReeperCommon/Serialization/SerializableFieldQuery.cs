using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    public class SerializableFieldQuery : IFieldInfoQuery
    {
        public IEnumerable<FieldInfo> Get(object target)
        {
            if (target == null) throw new ArgumentNullException("target");
            
            // return public or private fields marked [Persistent], excluding those marked
            // [GameSerialized] (ksp should handle serialization for those and doesn't need our help)
            return target.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                           BindingFlags.FlattenHierarchy)
                .Where(
                    fi =>
                        fi.GetCustomAttributes(true).Any(attr => attr is Persistent) &&
                        fi.GetCustomAttributes(true).All(attr => !(attr is GameSerializedAttribute)));
        }
    }
}
