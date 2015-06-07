using System.IO;
using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Repositories
{
    public interface IResourceRepository
    {
        Maybe<byte[]> GetRaw(string identifier);
        Maybe<Material> GetMaterial(string identifier);
        Maybe<Texture2D> GetTexture(string identifier);
        Maybe<AudioClip> GetClip(string identifier);
        Maybe<Stream> GetStream(string identifier);
    }
}
