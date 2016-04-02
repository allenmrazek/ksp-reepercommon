using System;

namespace ReeperCommon.Logging
{
    public interface ILog
    {
        void Debug(string format, params string[] args);
        void Debug(Func<string> message);
 
        void Normal(string format, params string[] args);
        void Normal(Func<string> message);
 
        void Warning(string format, params string[] args);
        void Warning(Func<string> message);
 
        void Error(string format, params string[] args);
        void Error(Func<string> message);
 
        void Performance(string format, params string[] args);
        void Performance(Func<string> message);
 
        void Verbose(string format, params string[] args);
        void Verbose(Func<string> message);
        
        ILog CreateTag(string tag);
 
    }
}
