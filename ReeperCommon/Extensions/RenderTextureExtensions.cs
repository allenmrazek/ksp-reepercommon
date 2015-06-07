using UnityEngine;

namespace ReeperCommon.Extensions
{
    public static class RenderTextureExtensions
    {
        public static Texture2D Capture(this RenderTexture target)
        {
            var old = RenderTexture.active;

            var texture = new Texture2D(target.width, target.height, TextureFormat.ARGB32, false);

            var rt = RenderTexture.GetTemporary(target.width, target.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB, 1);
            Graphics.Blit(target, rt);

            RenderTexture.active = rt;

            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);

            RenderTexture.active = old;
            RenderTexture.ReleaseTemporary(rt);

            return texture;
        }
    }
}
