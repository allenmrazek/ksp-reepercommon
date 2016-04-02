using System;

namespace ReeperCommon.Logging
{
    public class StandardLog : BaseLog
    {
        public StandardLog(string name = "") : base(name)
        {
        }

        public override void Debug(string format, params string[] args)
        {

        }

        public override void Debug(Func<string> message)
        {
            
        }


        public override void Normal(string format, params string[] args)
        {
            UnityEngine.Debug.Log(DoFormat(format, args));
        }

        public override void Normal(Func<string> message)
        {
            Normal(message());
        }


        public override void Warning(string format, params string[] args)
        {
            UnityEngine.Debug.LogWarning(DoFormat(format, args));
        }


        public override void Warning(Func<string> message)
        {
            Warning(message());
        }


        public override void Error(string format, params string[] args)
        {
            UnityEngine.Debug.LogError(DoFormat(format, args));
        }

        public override void Error(Func<string> message)
        {
            Error(message());
        }


        public override void Performance(string format, params string[] args)
        {

        }


        public override void Performance(Func<string> message)
        {
            Performance(message());
        }


        public override void Verbose(string format, params string[] args)
        {
            
        }

        public override void Verbose(Func<string> message)
        {
            Verbose(message());
        }
    }
}
