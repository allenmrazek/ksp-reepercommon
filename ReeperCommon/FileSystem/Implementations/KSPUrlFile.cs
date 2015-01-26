using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem.Implementations
{
    public class KSPUrlFile : IUrlFile
    {
        private readonly UrlDir.UrlFile _file;

        public KSPUrlFile(UrlDir.UrlFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            _file = file;
        }

        public string FullPath
        {
            get { return _file.fullPath; }
        }

        public string Extension
        {
            get { return _file.fileExtension; }
        }

        public string Name { get { return _file.name; } }
        public string Url { get { return _file.url; }}
    }
}
