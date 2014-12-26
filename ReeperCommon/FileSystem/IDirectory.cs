using System.Collections.Generic;

namespace ReeperCommon.FileSystem
{
    public interface IDirectory
    {
        IDirectory Directory(string url);
        

        bool FileExists(string url);
        bool DirectoryExists(string url);

        IFile File(string url);
        IEnumerable<IFile> Files();
        IEnumerable<IFile> Files(string extension);
        IEnumerable<IFile> RecursiveFiles();
        IEnumerable<IFile> RecursiveFiles(string extension);

        IEnumerable<IDirectory> Directories();

        IDirectory Parent { get; }
        string Path { get; }
    }
}
