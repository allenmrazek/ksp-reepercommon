using System.Collections.Generic;
using ReeperCommon.Containers;

namespace ReeperCommon.FileSystem
{
    public interface IDirectory
    {
        Maybe<IDirectory> Directory(string url);
        

        bool FileExists(string url);
        bool DirectoryExists(string url);

        Maybe<IFile> File(string url);
        IEnumerable<IFile> Files();
        IEnumerable<IFile> Files(string extension);
        IEnumerable<IFile> RecursiveFiles();
        IEnumerable<IFile> RecursiveFiles(string extension);

        IEnumerable<IDirectory> Directories();

        Maybe<IDirectory> Parent { get; }
        string FullPath { get; }
        string Url { get; }
        IUrlDir UrlDir { get; }
    }
}
