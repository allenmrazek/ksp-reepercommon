using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
    public class Vector3Surrogate : FieldSurrogateBase<Vector3>
    {
        protected override string GetFieldContentsAsString(Vector3 instance)
        {
            return KSPUtil.WriteVector(instance);
        }

        protected override Vector3 GetFieldContentsFromString(string value)
        {
            return KSPUtil.ParseVector3(value);
        }
    }
}
