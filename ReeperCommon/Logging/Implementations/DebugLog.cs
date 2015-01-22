namespace ReeperCommon.Logging.Implementations
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
    }
}
