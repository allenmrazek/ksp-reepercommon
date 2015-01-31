
using System.Collections;
using System.Collections.Generic;
using ReeperCommon.Extensions;
using ReeperCommon.Extensions.Object;

namespace ReeperCommon.Containers
{
    public struct Maybe<T> : IEnumerable<T>
    {
        private IEnumerable<T> _values;

        public static Maybe<T> None { get { return new Maybe<T>(); } }

        public static Maybe<T> With(T value)
        {
            var maybe = new Maybe<T> {_values = new T[] {value}};
            return maybe;
        }

        public IEnumerator<T> GetEnumerator()
        {
            LazyInitialize();

            return _values.GetEnumerator();
        }

        private void LazyInitialize()
        {
            if (_values.IsNull())
                _values = new T[] { };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
