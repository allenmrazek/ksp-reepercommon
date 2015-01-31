using System;

namespace ReeperCommon.FileSystem.Implementations
{
    public class KSPUrlIdentifier : IUrlIdentifier
    {
        private readonly string _url;
        private readonly string _path;


        public KSPUrlIdentifier(string url)
        {
            if (url == null) throw new ArgumentNullException("url");

            url = url.Trim('\\', '/');

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url is empty");

            if (url.StartsWith("."))
                throw new ArgumentException("url cannot start with a period");

            Parts = url.Split('/', '\\');

            _path = url;
            _url = "/" + (System.IO.Path.HasExtension(url) ? System.IO.Path.GetFileName(url) : url);
        }



        public string this[int i] 
        {
            get { return Parts[i]; }
        }



        public string Url { get { return _url; } }
        public string Path { get { return _path; } }
        public int Depth { get { return Parts.Length;  }}
        public string[] Parts { get; private set; }
    }
}