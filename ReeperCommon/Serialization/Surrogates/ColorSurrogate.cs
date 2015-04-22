using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
    public class ColorSurrogate : FieldSurrogateBase<Color>
    {
        protected override string GetFieldContentsAsString(Color instance)
        {
            return ConfigNode.WriteColor(instance);
        }

        protected override Color GetFieldContentsFromString(string value)
        {
            return ConfigNode.ParseColor(value);
        }
    }
}
