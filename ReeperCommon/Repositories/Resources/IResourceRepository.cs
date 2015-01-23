using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources
{
    public interface IResourceRepository
    {
        Maybe<byte[]> GetRaw(string identifier);
        Maybe<Material> GetMaterial(string identifier);
        Maybe<Texture2D> GetTexture(string identifier);
        Maybe<AudioClip> GetClip(string identifier);
    }
}
