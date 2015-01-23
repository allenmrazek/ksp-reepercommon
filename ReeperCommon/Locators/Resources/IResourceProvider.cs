using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Locators.Resources
{
    public interface IResourceProvider
    {
        Maybe<Texture2D> GetTexture(string identifier);
        Maybe<Material> GetMaterial(string identifier);
        Maybe<string> GetResourceAsString(string identifier);
        Maybe<byte[]> GetResourceRaw(string identifier);
    }
}
