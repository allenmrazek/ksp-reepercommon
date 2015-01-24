using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources.Implementations
{
    public class ResourceFromGameDatabase : IResourceRepository
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

        public override string ToString()
        {
            return "ResourceFromGameDatabase:" + System.Environment.NewLine + String.Join(System.Environment.NewLine,
                GameDatabase.Instance.databaseTexture
                    .Select(ti => "Texture: " + ti.name)
                    .Union(
                        GameDatabase.Instance.databaseAudio.Select(ac => "AudioClip: " + ac.name))
                    .Union(
                        GameDatabase.Instance.databaseModel.Select(m => "Model: " + m.name))
                    .ToArray());
        }
    }
}
