using System;

namespace ReeperCommon.Logging
{
    public class VerboseLog : StandardLog
    {
        public VerboseLog(string name = "") : base(name)
        {
        }

        public override void Performance(string format, params string[] args)
        {
            Normal(string.Format("[Performance] {0}", format), args);
        }

        public override void Performance(Func<string> message)
        {
            Performance(message());
        }

        public override void Verbose(string format, params string[] args)
        {
            Normal(format, args);
        }

        public override void Verbose(Func<string> message)
        {
            Verbose(message());
        }
    }
}
