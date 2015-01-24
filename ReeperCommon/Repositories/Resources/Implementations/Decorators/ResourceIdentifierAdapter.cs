using System;
using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources.Implementations.Decorators
{
    public class ResourceIdentifierAdapter : IResourceRepository
    {
        private readonly IResourceRepository _repository;
        private readonly Func<string, string> _transformer;

        public ResourceIdentifierAdapter(Func<string, string> transformer, IResourceRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (transformer == null) throw new ArgumentNullException("transformer");

            _repository = repository;
            _transformer = transformer;
        }

        public Maybe<byte[]> GetRaw(string identifier)
        {
            return _repository.GetRaw(_transformer(identifier));
        }

        public Maybe<Material> GetMaterial(string identifier)
        {
            return _repository.GetMaterial(_transformer(identifier));
        }

        public Maybe<Texture2D> GetTexture(string identifier)
        {
            return _repository.GetTexture(_transformer(identifier));
        }

        public Maybe<AudioClip> GetClip(string identifier)
        {
            return _repository.GetClip(_transformer(identifier));
        }

        public override string ToString()
        {
            return _repository.ToString();
        }
    }
}
