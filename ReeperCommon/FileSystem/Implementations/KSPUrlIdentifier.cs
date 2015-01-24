namespace ReeperCommon.FileSystem.Implementations
{
    internal class KSPUrlIdentifier
    {
        public readonly string Url;
        public readonly string[] Parts;

        public KSPUrlIdentifier(string url)
        {
            url = url.Trim('\\', '/');

            Url = url;
            Parts = url.Split('/', '\\');
        }

        public string this[int i] 
        {
            get { return Parts[i]; }
        }

        public int Depth { get { return Parts.Length;  }}
    }
}