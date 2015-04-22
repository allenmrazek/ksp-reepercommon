using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
    public class QuaternionSurrogate : FieldSurrogateBase<Quaternion>
    {
        protected override string GetFieldContentsAsString(Quaternion instance)
        {
            return KSPUtil.WriteQuaternion(instance);
        }

        protected override Quaternion GetFieldContentsFromString(string value)
        {
            return KSPUtil.ParseQuaternion(value);
        }
    }
}
