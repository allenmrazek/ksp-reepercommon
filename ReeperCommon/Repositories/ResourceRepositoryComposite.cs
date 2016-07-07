using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Repositories
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


        public Maybe<Stream> GetStream(string identifier)
        {
            foreach (var p in _providers)
            {
                var result = p.GetStream(identifier);
                if (result.Any()) return result;
            }

            return Maybe<Stream>.None;
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


        public override string ToString()
        {
            return "Resource Repository Composite:" + Environment.NewLine +
                   _providers
                        .Select(p => p.ToString())
                        .Aggregate((p1, p2) => string.Format("{0}{1}{1}{2}", p1, Environment.NewLine, p2));

        }
    }
}
