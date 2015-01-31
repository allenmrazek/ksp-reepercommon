using System.Text;

namespace ReeperCommon.Logging.Implementations
{
    public abstract class BaseLog : ILog
    {
        protected readonly string _LogName;

        protected BaseLog(string name = "")
        {
            _LogName = string.IsNullOrEmpty(name) ? System.Reflection.Assembly.GetExecutingAssembly().GetName().Name : name;
            //_ClassName = this.GetType().Name;
        }

        public abstract void Debug(string format, params string[] args);
        public abstract void Normal(string format, params string[] args);
        public abstract void Warning(string format, params string[] args);
        public abstract void Error(string format, params string[] args);
        public abstract void Performance(string format, params string[] args);
        public abstract void Verbose(string format, params string[] args);

        public ILog CreateTag(string tag)
        {
            return new TaggedLog(this, tag);
        }

        protected virtual string DoFormat(string format, params string[] args)
        {
            var builder = new StringBuilder(_LogName, format.Length + args.Length * 7);
            builder.Append(": ");
            builder.AppendFormat(format, args);

            return builder.ToString();
        }
    }
}
