namespace ReeperCommon.Logging
{
    public interface ILog
    {
        void Debug(string format, params string[] args);
        void Normal(string format, params string[] args);
        void Warning(string format, params string[] args);
        void Error(string format, params string[] args);
        void Performance(string format, params string[] args);
        void Verbose(string format, params string[] args);

        ILog CreateTag(string tag);
        
    }
}
