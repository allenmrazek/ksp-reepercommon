namespace ReeperCommon.Logging.Implementations
{
    public class NothingLog : Logging.Log
    {
        internal NothingLog()
        {
        }

        public override void Debug(string format, params string[] args)
        {

        }

        public override void Normal(string format, params string[] args)
        {

        }

        public override void Warning(string format, params string[] args)
        {

        }

        public override void Error(string format, params string[] args)
        {

        }

        public override void Performance(string format, params string[] args)
        {

        }

        public override void Verbose(string format, params string[] args)
        {

        }
    }
}
