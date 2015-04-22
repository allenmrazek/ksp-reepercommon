using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
    public class Vector2Surrogate : FieldSurrogateBase<Vector2>
    {
        protected override string GetFieldContentsAsString(Vector2 instance)
        {
            return KSPUtil.WriteVector(instance);
        }

        protected override Vector2 GetFieldContentsFromString(string value)
        {
            return KSPUtil.ParseVector2(value);
        }
    }
}
