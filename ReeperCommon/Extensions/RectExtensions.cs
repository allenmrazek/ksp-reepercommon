using UnityEngine;

namespace ReeperCommon.Extensions
{
    public static class RectExtensions
    {
        public static Rect Multiply(this Rect rect, Matrix4x4 matrix)
        {
            var topLeft = new Vector3(rect.x, rect.y, 0f);
            var topRight = new Vector3(rect.x + rect.width, rect.y, 0f);
            var bottomLeft = new Vector3(rect.x, rect.y + rect.height, 0f);
            var bottomRight = new Vector3(rect.x + rect.width, rect.y + rect.height, 0f);

            topLeft = matrix.MultiplyPoint3x4(topLeft);
            topRight = matrix.MultiplyPoint3x4(topRight);
            bottomLeft = matrix.MultiplyPoint3x4(bottomLeft);
            bottomRight = matrix.MultiplyPoint3x4(bottomRight);

            return new Rect(topLeft.x, topLeft.y, bottomRight.x - bottomLeft.x, bottomRight.y - topRight.y);
        }

        public static Rect Invert(this Rect rect, Matrix4x4 matrix)
        {
            return Multiply(rect, matrix.inverse);
        }

        public static Rect MultiplyScale(this Rect rect, Matrix4x4 matrix)
        {
            return new Rect(rect.x, rect.y,
                rect.width*matrix.GetRow(0).x,
                rect.height*matrix.GetRow(1).y);
        }

        public static Rect InvertScale(this Rect rect, Matrix4x4 matrix)
        {
            return new Rect(rect.x, rect.y, rect.width/matrix.GetRow(0).x, rect.height/matrix.GetRow(1).y);
        }
    }
}
