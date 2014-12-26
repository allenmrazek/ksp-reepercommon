using System;
using System.IO;
using ReeperCommon.Extensions;

namespace ReeperCommon.FileSystem.Implementations
{
    public class KSPFile : IFile
    {
        private readonly UrlDir.UrlFile _file;
        private readonly FileInfo _info;
        private readonly IDirectory _directory;

        public KSPFile(IDirectory directory, UrlDir.UrlFile file)
        {
            if (directory.IsNull())
                throw new ArgumentNullException("directory");

            if (file.IsNull())
                throw new ArgumentNullException("file");

            _directory = directory;
            _info = new System.IO.FileInfo(file.fullPath);
            
            if (_info.IsNull()) throw new FileNotFoundException(file.fullPath);

            _file = file;    
        }

        public UrlDir.UrlFile UrlFile
        {
            get { return _file; }
        }

        public FileInfo Info
        {
            get { return _info; }
        }

        public IDirectory Directory
        {
            get { return _directory; }
        }

        public string Extension
        {
            get { return _file.fileExtension; }
        }

        public string FullPath
        {
            get { return _file.fullPath; }
        }

        public string Name
        {
            get
            {
                return _file.name.Trim('/', '\\');
            }
        }

        public string FileName
        {
            get
            {
                return string.IsNullOrEmpty(Extension) ? Name : Name + "." + Extension;
            }
        }
    }
}
