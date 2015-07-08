using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class Vector2Surrogate : SurrogateToSingleValueBase<Vector2>
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
