﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    // note: private members inherited by target type will be ignored
    public class SerializableFieldQuery : IFieldInfoQuery
    {
        public IEnumerable<FieldInfo> Get(object target)
        {
            if (target == null) throw new ArgumentNullException("target");
            
            return target.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(
                    fi =>
                        fi.GetCustomAttributes(true).Any(attr => attr is ReeperPersistentAttribute));
        }
    }
}
