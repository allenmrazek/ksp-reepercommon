namespace ReeperCommon.Logging.Implementations
{
    public class StandardLog : Logging.BaseLog
    {
        public StandardLog(string name = "") : base(name)
        {
        }

        public override void Debug(string format, params string[] args)
        {

        }

        public override void Normal(string format, params string[] args)
        {
            UnityEngine.Debug.Log(DoFormat(format, args));
        }

        public override void Warning(string format, params string[] args)
        {
            UnityEngine.Debug.LogWarning(DoFormat(format, args));
        }

        public override void Error(string format, params string[] args)
        {
            UnityEngine.Debug.LogError(DoFormat(format, args));
        }

        public override void Performance(string format, params string[] args)
        {

        }

        public override void Verbose(string format, params string[] args)
        {
            
        }
    }
}
