namespace ReeperCommon.FileSystem
{
    public interface IUrlIdentifier
    {
        string Url { get; }
        string Path { get; }
        int Depth { get; }
        string[] Parts { get; }

        string this[int i] { get; }
    }
}
