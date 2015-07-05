using System;

namespace ReeperCommon.Serialization.Exceptions
{
    public class ConfigNodeDuplicateKeyException : Exception
    {
        public ConfigNodeDuplicateKeyException(string key) : base("ConfigNode already contains key \"" + key + "\"")
        {
            
        }
    }
}
