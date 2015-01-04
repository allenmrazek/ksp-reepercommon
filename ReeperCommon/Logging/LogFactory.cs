using System;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;

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
        public static Logging.Log Create(LogLevel level)
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

        public static Logging.Log Create(ConfigNode node)
        {
            if (node.HasValue("Level"))
            {
                var level = node.Parse("Level", LogLevel.Standard);

                return Create(level);
            }

            return Create(LogLevel.Standard);
        }
    }
}
