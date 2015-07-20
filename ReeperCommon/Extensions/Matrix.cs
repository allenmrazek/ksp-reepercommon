using UnityEngine;

namespace ReeperCommon.Extensions
{
// ReSharper disable once UnusedMember.Global
    public static class MatrixExtensions
    {
// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedMember.Global
        public static Vector2 GetScaleXY(this Matrix4x4 matrix)
        {
            return new Vector2(matrix.GetRow(0).x, matrix.GetRow(1).y);
        }
    }
}
