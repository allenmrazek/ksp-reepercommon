using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.Log.Implementations
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
