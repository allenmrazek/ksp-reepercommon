using System;

namespace ReeperCommon.Logging
{
    public class NothingLog : BaseLog
    {
        public override void Debug(string format, params string[] args)
        {
        }

        public override void Debug(Func<string> message)
        {
        }


        public override void Normal(string format, params string[] args)
        {
        }


        public override void Normal(Func<string> message)
        {
        }


        public override void Verbose(string format, params string[] args)
        {
        }

        public override void Verbose(Func<string> message)
        {
        }


        public override void Warning(string format, params string[] args)
        {
        }

        public override void Warning(Func<string> message)
        {
        }


        public override void Error(string format, params string[] args)
        {
        }

        public override void Error(Func<string> message)
        {
        }


        public override void Performance(string format, params string[] args)
        {
        }

        public override void Performance(Func<string> message)
        {
        }
    }
}
