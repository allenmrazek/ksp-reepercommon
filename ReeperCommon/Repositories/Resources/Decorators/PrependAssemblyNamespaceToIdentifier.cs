//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using ReeperCommon.Containers;
//using UnityEngine;

//namespace ReeperCommon.Locators.Resources.Decorators.Locator
//{
//    public class PrependAssemblyNamespaceToIdentifier : IResourceLocator
//    {
//        private readonly Assembly _assembly;
//        private readonly IResourceLocator _locator;

//        public PrependAssemblyNamespaceToIdentifier(Assembly assembly, IResourceLocator locator)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");
//            if (locator == null) throw new ArgumentNullException("locator");
//            _assembly = assembly;
//            _locator = locator;
//        }


//        private string PrependNamespace(string identifier)
//        {
//            return _assembly.GetName().Name + "." + identifier;
//        }



//        public Maybe<byte[]> GetRaw(string identifier)
//        {
//            return _locator.GetRaw(PrependNamespace(identifier));
//        }

//        public Maybe<Material> GetMaterial(string identifier)
//        {
//            return _locator.GetMaterial(PrependNamespace(identifier));
//        }

//        public Maybe<Texture2D> GetTexture(string identifier)
//        {
//            return _locator.GetTexture(PrependNamespace(identifier));
//        }

//        public Maybe<AudioClip> GetClip(string identifier)
//        {
//            return _locator.GetClip(PrependNamespace(identifier));
//        }

//        public IEnumerable<string> GetPossibilities()
//        {
//            return _locator.GetPossibilities();
//        }
//    }
//}
