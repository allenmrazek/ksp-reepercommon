using System;
using System.Linq;
using System.Text;

namespace ReeperCommon.Logging
{
    public abstract class BaseLog : ILog
    {
        protected readonly string LogName;

        protected BaseLog(string name = "")
        {
            LogName = string.IsNullOrEmpty(name) ? System.Reflection.Assembly.GetExecutingAssembly().GetName().Name : name;
        }

        public abstract void Debug(string format, params string[] args);
        public abstract void Debug(Func<string> message);
 
        public abstract void Normal(string format, params string[] args);
        public abstract void Normal(Func<string> message);
 
        public abstract void Warning(string format, params string[] args);
        public abstract void Warning(Func<string> message);
 
        public abstract void Error(string format, params string[] args);
        public abstract void Error(Func<string> message);
 
        public abstract void Performance(string format, params string[] args);
        public abstract void Performance(Func<string> message);
 
        public abstract void Verbose(string format, params string[] args);
        public abstract void Verbose(Func<string> message);
 

        public ILog CreateTag(string tag)
        {
            return new TaggedLog(this, tag);
        }

        protected virtual string DoFormat(string format, params string[] args)
        {
            var builder = new StringBuilder(LogName, format.Length + args.Length * 7);
            builder.Append(": ");

            builder.AppendFormat(format, args);

            return builder.ToString();
        }
    }
}
