using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources.Implementations
{
    class ResourceFromGameDatabase : IResourceRepository
    {
        public Maybe<byte[]> GetRaw(string identifier)
        {
            return Maybe<byte[]>.None;
        }

        public Maybe<Material> GetMaterial(string identifier)
        {
            return Maybe<Material>.None;
        }

        public Maybe<Texture2D> GetTexture(string identifier)
        {
            var tex = GameDatabase.Instance.GetTexture(identifier, false);

            return tex.IsNull() ? Maybe<Texture2D>.None : Maybe<Texture2D>.With(tex);
        }

        public Maybe<AudioClip> GetClip(string identifier)
        {
            var ac = GameDatabase.Instance.GetAudioClip(identifier);

            return ac.IsNull() ? Maybe<AudioClip>.None : Maybe<AudioClip>.With(ac);
        }
    }
}
