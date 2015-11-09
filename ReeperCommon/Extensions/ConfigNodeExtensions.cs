using System;
using System.IO;
using System.Linq;
using ReeperCommon.Containers;

namespace ReeperCommon.Extensions
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
        public static T ParseEnum<T>(this ConfigNode node, string valueName, T defaultValue)
        {
            if (!node.HasValue(valueName))
                return defaultValue;

            var value = node.GetValue(valueName);

            return (T)Enum.Parse(typeof(T), value, true);
        }



        /// <summary>
        /// Parse a value from a ConfigNode
        /// </summary>
        /// <param name="node"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Parse<T>(this ConfigNode node, string valueName, T defaultValue)
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
                throw new MissingMethodException(typeof (T).FullName, "TryParse");
                
            var args = new object[] { value, default(T) };

            if ((bool)method.Invoke(null, args))
                return (T)args[1];    

            throw new Exception(string.Format("Failed to invoke TryParse with {0}", value));
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
            if (node.HasValue(valueName))
                node.SetValue(valueName, value.ToString());
            else node.AddValue(valueName, value);
        }


        public static void Write(this ConfigNode node, string fullPath, string header)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentException("Invalid path: " + fullPath, "fullPath");

            
            using (var stream = new StreamWriter(new FileStream(fullPath, FileMode.Create, FileAccess.Write)))
            {
                if (!string.IsNullOrEmpty(header))
                    stream.WriteLine(header.StartsWith("\\\\") ? header : "\\\\ " + header.Trim());

                WriteNode(stream, node, 0);
            }
        }


        private static void WriteNode(TextWriter stream, ConfigNode node, int depth)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (node == null) throw new ArgumentNullException("node");
            if (depth < 0)
                throw new ArgumentException("Invalid depth: " + depth, "depth");


            WriteNestedLine(stream, depth, node.name.Trim());
            WriteNestedLine(stream, depth, "{");

            {
                // write all values of this node
                foreach (ConfigNode.Value value in node.values)
                    WriteNestedLine(stream, depth + 1,
                        value.name.Trim() + " = " + value.value.Trim());
            }

            if (node.CountValues > 0 && node.CountNodes > 0)
                stream.Write(stream.NewLine);

            { // write all sub nodes of this node
                foreach (ConfigNode sub in node.nodes)
                    WriteNode(stream, sub, depth + 1);
            }

            WriteNestedLine(stream, depth, "}");
        }


        private static void WriteNestedLine(TextWriter stream, int depth, string data)
        {
            stream.Write(new string('\t', depth));
            stream.Write(data);
            stream.Write(stream.NewLine);
        }


        public static string ToSafeString(this ConfigNode config)
        {
            return config.ToString().Replace("{", "{{").Replace("}", "}}");
        }
    }
}
