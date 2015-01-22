using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Locators.Resources.Implementations
{
    public class ResourceProvider : IResourceProvider
    {
        private readonly IResourceLocator _locator;
        private readonly ILog _log;

        public ResourceProvider(IResourceLocator locator, ILog log)
        {
            if (locator == null) throw new ArgumentNullException("locator");
            if (log == null) throw new ArgumentNullException("log");
            _locator = locator;
            _log = log;
        }



        public Maybe<Texture2D> GetTexture(string identifier)
        {
            var data = _locator.FindResource(identifier);
            if (!data.Any())
            {
                _log.Verbose("Failed to find '{0}'", identifier);
                return Maybe<Texture2D>.None;
            }
            var texture = new Texture2D(1, 1);

            return texture.LoadImage(data.First()) ? Maybe<Texture2D>.With(texture) : Maybe<Texture2D>.None;
        }



        public Maybe<Material> GetMaterial(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
