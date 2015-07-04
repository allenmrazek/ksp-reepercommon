
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Extensions;

namespace ReeperCommon.Containers
{
    public class Maybe<T> : IEnumerable<T>
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>();


        private IEnumerable<T> _values;

        public static Maybe<T> None { get { return Nothing; } }

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

        public T Or(T other)
        {
            return _values.Any() ? _values.Single() : other;
        }
    }


    public static class ToMaybeExtension
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Maybe<T>.With(value);
        }
    }


    public static class IfNotNullExtension
    {
        public static U IfNotNull<T, U>(this Maybe<T> value, Func<T, U> func) 
            where T:class
            where U:class
        {
            if (!value.Any() || value.Single() == null) return null;

            return value.Any()
                ? func(value.Single())
                : null;
        }
    }
}
