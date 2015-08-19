using System.Collections.Generic;
using ReeperCommon.Containers;

namespace ReeperCommon.FileSystem
{
    public interface IDirectory
    {
        Maybe<IDirectory> Directory(IUrlIdentifier url);


        bool FileExists(IUrlIdentifier url);
        bool DirectoryExists(IUrlIdentifier url);

        Maybe<IFile> File(IUrlIdentifier url);
        Maybe<IFile> File(string filename);
        IEnumerable<IFile> Files();
        IEnumerable<IFile> Files(string extension);
        IEnumerable<IFile> RecursiveFiles();
        IEnumerable<IFile> RecursiveFiles(string extension);

        IEnumerable<IDirectory> Directories();
        IEnumerable<IDirectory> RecursiveDirectories();

        Maybe<IDirectory> Parent { get; }
        string FullPath { get; }
        string Url { get; }
        IUrlDir UrlDir { get; }
        string Name { get; }
    }
}
