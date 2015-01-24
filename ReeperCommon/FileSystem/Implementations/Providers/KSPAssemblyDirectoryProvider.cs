//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;

//namespace ReeperCommon.FileSystem.Implementations
//{
//    class KSPAssemblyDirectoryProvider : IGameDataPathQuery
//    {
//        private readonly Assembly _assembly;
//        private readonly IDirectory _gameData;
//        private UrlDir _directory;

//        public KSPAssemblyDirectoryProvider(Assembly assembly, IDirectory gameData)
//        {
//            _assembly = assembly;
//            _gameData = gameData;
//        }

//        public UrlDir Directory()
//        {
//            LazyInitialize();
//            return _directory;
//        }

//        private void LazyInitialize()
//        {
//            if (_directory.IsNull())
//            {
//                var filename = System.IO.Path.GetFileName(_assembly.CodeBase);
//                var ext = 
//            }
//        }

//        private Maybe<IFile> Search(string target, IDirectory @in)
//        {
//            if (@in == null) throw new ArgumentNullException("in");


//            if (System.IO.Path.IsPathRooted(target))
//            {
//                var nextDir = System.IO.Path.GetPathRoot(target);
//            }
//            else
//            {
//                return @in.FileExists(target) ? Maybe<IFile>.With(@in.File(target)) : Maybe<IFile>.None;
//            }
//        }
//    }
//}
