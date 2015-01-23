using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Locators.Resources.Implementations
{
    public class ResourceProvider : IResourceProvider
    {
        private readonly IResourceLocator _locator;



        public ResourceProvider(IResourceLocator locator)
        {
            if (locator == null) throw new ArgumentNullException("locator");
            _locator = locator;
        }



        public Maybe<Texture2D> GetTexture(string identifier)
        {
            var data = _locator.FindResource(identifier);
            if (!data.Any())
                return Maybe<Texture2D>.None;
            var texture = new Texture2D(1, 1);

            return texture.LoadImage(data.First()) ? Maybe<Texture2D>.With(texture) : Maybe<Texture2D>.None;
        }



        public Maybe<Material> GetMaterial(string identifier)
        {
            var strData = GetResourceAsString(identifier);
            if (!strData.Any())
                return Maybe<Material>.None;

            var material = new Material(strData.Single());

            return !material.IsNull() ? Maybe<Material>.With(material) : Maybe<Material>.None;
        }



        public Maybe<string> GetResourceAsString(string identifier)
        {
            var raw = GetResourceRaw(identifier);

            return raw.Any() ? Maybe<string>.With(Convert.ToString(raw.Single())) : Maybe<string>.None;
        }



        public Maybe<byte[]> GetResourceRaw(string identifier)
        {
            return _locator.FindResource(identifier);
        }
    }
}
