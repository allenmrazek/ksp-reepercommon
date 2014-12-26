using System.Text;

namespace ReeperCommon.Log
{
    public abstract class Log
    {
        protected readonly string _AssemblyName;
        protected readonly string _ClassName;

        internal Log()
        {
            _AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            _ClassName = this.GetType().Name;
        }

        public abstract void Debug(string format, params string[] args);
        public abstract void Normal(string format, params string[] args);
        public abstract void Warning(string format, params string[] args);
        public abstract void Error(string format, params string[] args);
        public abstract void Performance(string format, params string[] args);
        public abstract void Verbose(string format, params string[] args);

        protected virtual string DoFormat(string format, params string[] args)
        {
            var builder = new StringBuilder(_ClassName, format.Length + args.Length * 7);
            builder.Append(": ");
            builder.AppendFormat(format, args);

            return builder.ToString();
        }
    }
}
