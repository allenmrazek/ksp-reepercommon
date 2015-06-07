using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.DataObjects
{
    public class ColorHsl
    {

        private float _hue = 0f;

        public float Hue
        {
            get { return _hue; }
            set { _hue = Mathf.Clamp(value, 0f, 1f); }
        }


        private float _saturation = 0f;

        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = Mathf.Clamp(value, 0f, 1f); }
        }


        private float _lightness = 0f;

        public float Lightness
        {
            get { return _lightness; }
            set { _lightness = Mathf.Clamp(value, 0f, 1f); }
        }

        // Borrowed from: http://stackoverflow.com/questions/2353211/hsl-to-rgb-color-conversion
        public Color Color
        {
            get
            {
                var color = new Color(0f, 0f, 0f, 1f);

                if (Saturation < 0.0001f)
                {
                    color.r = color.g = color.b = Lightness;
                }
                else
                {
                    var q = Lightness < 0.5f
                        ? Lightness * (1 + Saturation)
                        : Lightness + Saturation - Lightness * Saturation;

                    var p = 2 * Lightness - q;

                    color.r = HueToRGB(p, q, Hue + 1f / 3);
                    color.g = HueToRGB(p, q, Hue);
                    color.b = HueToRGB(p, q, Hue - 1f / 3);
                }

                return color;
            }
            set
            {
                var newColorHsl = value.GetHSL();

                Hue = newColorHsl.Hue;
                Saturation = newColorHsl.Saturation;
                Lightness = newColorHsl.Lightness;
            }
        }


        public ColorHsl(float hue, float saturation, float lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }


        public ColorHsl(Color color):this(color.GetHSL())
        {
            
        }


        public ColorHsl(ColorHsl other)
        {
            Color = other.Color;
        }

        public override string ToString()
        {
            return string.Format("HSL(H = {0:0.##}, S = {1:0.##}, L = {2:0.##})", Hue, Saturation, Lightness);
        }


// ReSharper disable once InconsistentNaming
        private static float HueToRGB(float p, float q, float t)
        {
            if (t < 0) t += 1f;
            if (t > 1) t -= 1f;
            if (t < 1f / 6f) return p + (q - p) * 6f * t;
            if (t < 0.5f) return q;
            if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
            return p;
        }
    }
}
