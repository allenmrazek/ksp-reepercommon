using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Repositories.Resources.Implementations
{
    public class ResourceFromEmbeddedResource : IResourceRepository
    {
        private readonly Assembly _assembly;

        public ResourceFromEmbeddedResource(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            _assembly = assembly;
        }


        public Maybe<byte[]> GetRaw(string identifier)
        {
            var stream = _assembly.GetManifestResourceStream(identifier);

            if (stream == null || stream.Length == 0)
                return Maybe<byte[]>.None;
            

            var data = new byte[stream.Length];

            int read = 0;
            var ms = new MemoryStream();

            while ((read = stream.Read(data, 0, data.Length)) > 0)
                ms.Write(data, 0, read);

            return Maybe<byte[]>.With(data);
        }



        public Maybe<Material> GetMaterial(string identifier)
        {
            var data = GetRaw(identifier);

            if (!data.Any()) return Maybe<Material>.None;

            var material = new Material(Convert.ToString(data.Single()));

            return material.IsNull() ? Maybe<Material>.None : Maybe<Material>.With(material);
        }



        public Maybe<Texture2D> GetTexture(string identifier)
        {
            var data = GetRaw(identifier);

            if (!data.Any()) return Maybe<Texture2D>.None;

            var tex = new Texture2D(1, 1);

            return tex.LoadImage(data.Single()) ? Maybe<Texture2D>.With(tex) : Maybe<Texture2D>.None;
        }



        public Maybe<AudioClip> GetClip(string identifier)
        {
            return Maybe<AudioClip>.None;
        }
    }
}
