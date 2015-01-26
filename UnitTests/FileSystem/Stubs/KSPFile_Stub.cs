using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace UnitTests.FileSystem.Stubs
{
    class KSPFile_Stub : IFile
    {
        public IUrlFile UrlFile { get; private set; }
        public Maybe<FileInfo> Info { get; private set; }
        public IDirectory Directory { get; private set; }
        public string Extension { get; private set; }
        public string FullPath { get; private set; }
        public string Name { get; private set; }
        public string FileName { get; private set; }
        public string Url { get; private set; }
    }
}
