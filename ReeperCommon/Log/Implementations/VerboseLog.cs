using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.Log.Implementations
{
    public class VerboseLog : StandardLog
    {
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
