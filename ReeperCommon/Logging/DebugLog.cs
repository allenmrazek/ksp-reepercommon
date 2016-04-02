using System;

namespace ReeperCommon.Logging
{
    public class DebugLog : VerboseLog
    {
        public DebugLog(string name = "") : base(name)
        {
        }

        public override void Debug(string format, params string[] args)
        {
            Normal(format, args);
        }

        public override void Debug(Func<string> message)
        {
            Debug(message());
        }
    }
}
