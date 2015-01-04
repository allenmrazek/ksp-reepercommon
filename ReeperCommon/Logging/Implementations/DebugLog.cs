namespace ReeperCommon.Logging.Implementations
{
    public class DebugLog : VerboseLog
    {
        internal DebugLog()
        {
        }

        public override void Debug(string format, params string[] args)
        {
            Normal(format, args);
        }
    }
}
