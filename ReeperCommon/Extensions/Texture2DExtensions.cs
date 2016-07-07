using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ReeperCommon.Extensions
{
    public static class Texture2DExtensions
    {
        public static Texture2D Clone(this Texture2D original)
        {
            return Object.Instantiate(original) as Texture2D;
        }


        public static Texture2D CreateReadable(this Texture2D original, Material blitMaterial = null)
        {
            if (original == null) throw new ArgumentNullException("original");

            if (original.width == 0 || original.height == 0)
                throw new Exception("Invalid image dimensions");

            var finalTexture = new Texture2D(original.width, original.height);

            // isn't read or writeable ... we'll have to get tricksy
            var rt = RenderTexture.GetTemporary(original.width, original.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB, 1);

            if (blitMaterial == null)
                Graphics.Blit(original, rt);
            else Graphics.Blit(original, rt, blitMaterial);

            RenderTexture.active = rt;

            finalTexture.ReadPixels(new Rect(0, 0, finalTexture.width, finalTexture.height), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            finalTexture.Apply(true);

            return finalTexture;
        }


        public static void GenerateRandom(this Texture2D tex)
        {
            var pixels = tex.GetPixels32();

            for (int y = 0; y < tex.height; ++y)
                for (int x = 0; x < tex.width; ++x)
                    pixels[y * tex.width + x] = new Color(Random.Range(0f, 1f),
                                                            Random.Range(0f, 1f),
                                                            Random.Range(0f, 1f),
                                                            Random.Range(0f, 1f));

            tex.SetPixels32(pixels);
            tex.Apply();
        }


        public static Texture2D GenerateRandom(int w, int h)
        {
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
            tex.GenerateRandom();
            return tex;
        }


        public static void SetOpacity(this Texture2D original, int opacity, int mipLevel = 0)
        {
            opacity = Mathf.Clamp(opacity, 0, 255);

            try
            {
                var pixels = original.GetPixels32(mipLevel);

                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i].a = (byte) opacity;

                original.SetPixels32(pixels);
                original.Apply();
            }
            catch (UnityException)
            {
                // texture isn't readable
                try
                {
                    original.CreateReadable().SetOpacity(opacity, mipLevel);
                }
                catch
                {
                    throw new Exception("Original texture not readable and failed to create readable version");
                }
            }
        }


        public static void SetLightness(this Texture2D original, float lightness)
        {
            if (lightness < 0f || lightness > 1f)
                throw new ArgumentOutOfRangeException("lightness");

            TraversePixels(original, color =>
            {
                var hsl = color.GetHSL();

                hsl.Lightness = lightness;

                return new Color(hsl.Color.r, hsl.Color.g, hsl.Color.b, color.a);
            });
        }


        public static void ChangeLightness(this Texture2D original, float multiplier)
        {
            TraversePixels(original, color =>
            {
                var hsl = color.GetHSL();

                hsl.Lightness *= multiplier;

                return new Color(hsl.Color.r, hsl.Color.g, hsl.Color.b, color.a);
            });
        }



        private static void TraversePixels(Texture2D texture, Func<Color, Color> action)
        {
            if (texture == null) throw new ArgumentNullException("texture");
            if (action == null) throw new ArgumentNullException("action");
            if (!new[] {TextureFormat.ARGB32, TextureFormat.RGB24, TextureFormat.Alpha8}.Contains(texture.format))
                throw new Exception("Texture format must be ARGB32, RGB24 or Alpha8 for SetPixels to function");

            for (int i = 0; i < texture.mipmapCount; ++i)
            {
                var pixels = texture.GetPixels(i);

                for (int j = 0; j < pixels.Length; ++j)
                    pixels[j] = action(pixels[j]);

                texture.SetPixels(pixels, i);
            }

            
            texture.Apply();
        }





        public static Texture2D As2D(this Texture tex)
        {
            return tex as Texture2D;
        }


        public static Texture2D Rotate(this Texture2D texture, float angleDegrees)
        {
            if (texture == null) throw new ArgumentNullException("texture");

            var rotated = texture.Clone();   
            var angle = angleDegrees * Mathf.Deg2Rad;

            for (int mipLevel = 0; mipLevel < texture.mipmapCount; ++mipLevel)
            {
                int width = Math.Max(1, texture.width >> mipLevel);
                int height = Mathf.Max(1, texture.height >> mipLevel);
                float centerX = width * 0.5f;
                float centerY = height * 0.5f;
                var originalPixels = texture.GetPixels32(mipLevel);
                var pixels = Enumerable.Repeat((Color32)Color.clear, width * height).ToArray();

                for (int y = 0; y < width; ++y)
                    for (int x = 0; x < height; ++x)
                    {
                        var srcX = centerX + (x - centerX)*Mathf.Cos(angle) - (y - centerY)*Mathf.Sin(angle);
                        var srcY = centerY + (x - centerX)*Mathf.Sin(angle) + (y - centerY)*Mathf.Cos(angle);

                        if (srcX < 0f || srcX >= width) continue;
                        if (srcY < 0f || srcY >= height) continue;

                        pixels[y*width + x] = mipLevel == 0 ? (Color32)texture.GetPixelBilinear(srcX / width, srcY / height)
                            : originalPixels[Mathf.RoundToInt(y * width) + Mathf.RoundToInt(x)];
                    }

                rotated.SetPixels32(pixels, mipLevel);
            }

        
            rotated.Apply(texture.mipmapCount > 0);

            return rotated;
        }
    }
}
