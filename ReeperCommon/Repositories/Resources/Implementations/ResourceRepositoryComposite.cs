using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources.Implementations
{

    public class ResourceRepositoryComposite : IResourceRepository
    {
        private readonly IEnumerable<IResourceRepository> _providers;

        public ResourceRepositoryComposite(params IResourceRepository[] providers)
        {
            if (providers == null) throw new ArgumentNullException("providers");
            _providers = providers;
        }



        public Maybe<Texture2D> GetTexture(string identifier)
        {
            foreach (var p in _providers)
            {
                var result = p.GetTexture(identifier);
                if (result.Any()) return result;
            }

            return Maybe<Texture2D>.None;
        }



        public Maybe<Material> GetMaterial(string identifier)
        {
            foreach (var p in _providers)
            {
                var result = p.GetMaterial(identifier);
                if (result.Any()) return result;
            }

            return Maybe<Material>.None;
        }



        public Maybe<AudioClip> GetClip(string identifier)
        {
            foreach (var p in _providers)
            {
                var result = p.GetClip(identifier);
                if (result.Any()) return result;
            }

            return Maybe<AudioClip>.None;
        }



        public Maybe<byte[]> GetRaw(string identifier)
        {
            foreach (var p in _providers)
            {
                var result = p.GetRaw(identifier);
                if (result.Any()) return result;
            }

            return Maybe<byte[]>.None;
        }
    }
}
