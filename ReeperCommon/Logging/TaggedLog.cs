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

        public void Normal(string format, params string[] args)
        {
            _log.Normal(_name + ": " + format, args);
        }

        public void Warning(string format, params string[] args)
        {
            _log.Warning(_name + ": " + format, args);
        }

        public void Error(string format, params string[] args)
        {
            _log.Error(_name + ": " + format, args);
        }

        public void Performance(string format, params string[] args)
        {
            _log.Performance(_name + ": " + format, args);
        }

        public void Verbose(string format, params string[] args)
        {
            _log.Verbose(_name + ": " + format, args);
        }

        public ILog CreateTag(string tag)
        {
            return new TaggedLog(this, tag);
        }
    }
}
