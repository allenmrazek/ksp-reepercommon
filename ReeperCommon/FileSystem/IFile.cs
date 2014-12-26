namespace ReeperCommon.FileSystem
{
    public interface IFile
    {
        UrlDir.UrlFile UrlFile { get; }
        System.IO.FileInfo Info { get; }
        IDirectory Directory { get; }
        string Extension { get; }
        string FullPath { get; }
        string Name { get; }
        string FileName { get; } // includes extension (if any)
    }
}
