//using System;
//using System.Collections.Generic;
//using ReeperCommon.Containers;
//using UnityEngine;

//namespace ReeperCommon.Locators.Resources.Decorators.Locator
//{
//    public class TransformIdentifierSlashToDot : IResourceLocator
//    {
//        private readonly IResourceLocator _locator;

//        public TransformIdentifierSlashToDot(IResourceLocator locator)
//        {
//            if (locator == null) throw new ArgumentNullException("locator");
//            _locator = locator;
//        }

//        public Maybe<byte[]> FindResource(string identifier)
//        {
//            return _locator.FindResource(identifier.Replace('/', '.'));
//        }

//        public Maybe<byte[]> GetRaw(string identifier)
//        {
//            throw new NotImplementedException();
//        }

//        public Maybe<Material> GetMaterial(string identifier)
//        {
//            throw new NotImplementedException();
//        }

//        public Maybe<Texture2D> GetTexture(string identifier)
//        {
//            throw new NotImplementedException();
//        }

//        public Maybe<AudioClip> GetClip(string identifier)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<string> GetPossibilities()
//        {
//            return _locator.GetPossibilities();
//        }
//    }
//}
