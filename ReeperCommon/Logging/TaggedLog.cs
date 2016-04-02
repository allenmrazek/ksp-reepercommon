using System;

namespace ReeperCommon.Logging
{
    // decorator to make logs from different sources more apparent
    public class TaggedLog : ILog
    {
        private readonly ILog _log;
        private readonly string _name;

        public TaggedLog(ILog log, string name)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (name == null) throw new ArgumentNullException("name");
            _log = log;
            _name = name;
        }

        public void Debug(string format, params string[] args)
        {
            _log.Debug(_name + ": " + format, args);
        }

        public void Debug(Func<string> message)
        {
            Debug(message());
        }

        public void Normal(string format, params string[] args)
        {
            _log.Normal(_name + ": " + format, args);
        }

        public void Normal(Func<string> message)
        {
            Normal(message());
        }

        public void Warning(string format, params string[] args)
        {
            _log.Warning(_name + ": " + format, args);
        }

        public void Warning(Func<string> message)
        {
            Warning(message());
        }

        public void Error(string format, params string[] args)
        {
            _log.Error(_name + ": " + format, args);
        }

        public void Error(Func<string> message)
        {
            Error(message());
        }


        public void Performance(string format, params string[] args)
        {
            _log.Performance(_name + ": " + format, args);
        }

        public void Performance(Func<string> message)
        {
            Performance(message());
        }


        public void Verbose(string format, params string[] args)
        {
            _log.Verbose(_name + ": " + format, args);
        }

        public void Verbose(Func<string> message)
        {
            Verbose(message());
        }

        public ILog CreateTag(string tag)
        {
            return new TaggedLog(this, tag);
        }
    }
}
