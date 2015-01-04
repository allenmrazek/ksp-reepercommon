using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReeperCommon.Log.Implementations
{
    using Unity = UnityEngine.Debug;

    public class StandardLog : Log
    {
        internal StandardLog()
        {
        }

        public override void Debug(string format, params string[] args)
        {

        }

        public override void Normal(string format, params string[] args)
        {
            Unity.Log(DoFormat(format, args));
        }

        public override void Warning(string format, params string[] args)
        {
            Unity.LogWarning(DoFormat(format, args));
        }

        public override void Error(string format, params string[] args)
        {
            Unity.LogError(DoFormat(format, args));
        }

        public override void Performance(string format, params string[] args)
        {

        }

        public override void Verbose(string format, params string[] args)
        {
            
        }
    }
}
