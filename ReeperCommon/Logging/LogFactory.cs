using System;

namespace ReeperCommon.Logging
{
    public enum LogLevel
    {
        Standard,
        Verbose,
        Debug,
        DontLog
    }

    public static class LogFactory
    {
        public static ILog Create(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return new DebugLog();
                case LogLevel.Standard:
                    return new StandardLog();
                case LogLevel.Verbose:
                    return new VerboseLog();
                case LogLevel.DontLog:
                    return new NothingLog();
                default:
                    throw new NotImplementedException(level.ToString());
            }
        }
    }
}
