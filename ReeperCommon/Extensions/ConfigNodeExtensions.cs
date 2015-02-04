using System;

namespace ReeperCommon.Extensions.ConfigNode
{
    public static class ConfigNodeExtensions
    {
        /// <summary>
        /// Parse an Enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this global::ConfigNode node, string valueName, T defaultValue)
        {
            if (!node.HasValue(valueName))
                return defaultValue;

            var value = node.GetValue(valueName);

            var values = Enum.GetValues(typeof(T));

            return (T)Enum.Parse(typeof(T), value, true);
        }



        /// <summary>
        /// Parse a value from a ConfigNode
        /// </summary>
        /// <param name="node"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Parse<T>(this global::ConfigNode node, string valueName, T defaultValue)
        {
            if (!node.HasValue(valueName))
                return defaultValue;

            var value = node.GetValue(valueName);

            if (typeof(T) == typeof(string) || typeof(T) == typeof(String))
                return (T)(object)value;

            var method = typeof(T).GetMethod("TryParse", new[] {
                typeof (string),
                typeof(T).MakeByRefType()
            });

            if (method.IsNull())
                throw new Exception(string.Format("TryParse not found in {0}", typeof (T).Name));
                
            var args = new object[] { value, default(T) };

            if ((bool)method.Invoke(null, args))
                return (T)args[1];    
            else throw new Exception(string.Format("Failed to invoke TryParse with {0}", value));
        }



        /// <summary>
        /// Set a value in a ConfigNode. If the value already exists, the existing value is changed
        /// otherwise a new entry iscreated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        public static void Set<T>(this global::ConfigNode node, string valueName, T value)
        {
            // something seems to be broken with ConfigNode.SetValue so the
            // following is buggy:
            //if (!node.SetValue(valueName, value)) node.SetValue(valueName, value);

            if (node.HasValue(valueName))
                node.SetValue(valueName, value.ToString());
            else node.AddValue(valueName, value);
        }
    }
}
