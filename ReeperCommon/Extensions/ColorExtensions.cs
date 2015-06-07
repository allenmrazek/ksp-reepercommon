using System;
using ReeperCommon.DataObjects;
using UnityEngine;

namespace ReeperCommon.Extensions
{
    public static class ColorExtensions
    {
// ReSharper disable once InconsistentNaming
        // Borrowed from: http://stackoverflow.com/questions/2353211/hsl-to-rgb-color-conversion
        public static ColorHsl GetHSL(this Color color)
        {
            var max = Mathf.Max(color.r, color.g, color.b);
            var min = Mathf.Min(color.r, color.g, color.b);

            float h = 0f, s = 0f, l = (max + min)*0.5f;

            if (!(Math.Abs(max - min) > 0.0001f)) return new ColorHsl(h, s, l);

            var d = max - min;
            s = l > 0.5f ? d/(2f - max - min) : d/(max + min);

            if (Math.Abs(max - color.r) < 0.0001f)
            {
                h = (color.g - color.b)/d + (color.g < color.b ? 6 : 0);
            } else if (Math.Abs(max - color.g) < 0.0001f)
            {
                h = (color.b - color.r)/d + 2f;
            } else if (Math.Abs(max - color.b) < 0.0001f)
            {
                h = (color.r - color.g)/d + 4f;
            }

            h /= 6f;

            return new ColorHsl(h, s, l);
        }
    }
}
