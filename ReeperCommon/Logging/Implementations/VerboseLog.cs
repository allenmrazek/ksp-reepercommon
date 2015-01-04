namespace ReeperCommon.Logging.Implementations
{
    public class VerboseLog : StandardLog
    {
        internal VerboseLog()
        {
        }

        public override void Performance(string format, params string[] args)
        {
            Normal(string.Format("[Performance] {0}", format), args);
        }

        public override void Verbose(string format, params string[] args)
        {
            Normal(format, args);
        }
    }
}
