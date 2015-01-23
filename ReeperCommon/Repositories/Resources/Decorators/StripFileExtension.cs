//using System;
//using System.Collections.Generic;
//using System.IO;
//using ReeperCommon.Containers;
//using UnityEngine;

//namespace ReeperCommon.Locators.Resources.Decorators
//{
//    public class StripFileExtension : IResourceLocator
//    {
//        private readonly IResourceLocator _locator;


//        public StripFileExtension(IResourceLocator locator)
//        {
//            if (locator == null) throw new ArgumentNullException("locator");
//            _locator = locator;
//        }



//        private string StripExtension(string identifier)
//        {
//            if (!Path.HasExtension(identifier) || string.IsNullOrEmpty(identifier)) return identifier;

//            var dir = Path.GetDirectoryName(identifier) ?? "";
//            var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(identifier)).Replace('\\', '/');
            
//            return !string.IsNullOrEmpty(woExt) ? woExt : identifier;
//        }


//        public Maybe<Texture2D> GetTexture(string identifier)
//        {
//            return _locator.GetTexture(StripExtension(identifier));
//        }

//        public Maybe<Material> GetMaterial(string identifier)
//        {
//            return _locator.GetMaterial(StripExtension(identifier));
//        }

//        public Maybe<AudioClip> GetClip(string identifier)
//        {
//            return _locator.GetClip(StripExtension(identifier));
//        }

//        public IEnumerable<string> GetPossibilities()
//        {
//            return _locator.GetPossibilities();
//        }


//        public Maybe<byte[]> GetRaw(string identifier)
//        {
//            return _locator.GetRaw(StripExtension(identifier));
//        }
//    }
//}
