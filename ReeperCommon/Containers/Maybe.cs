
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Extensions;

namespace ReeperCommon.Containers
{
    public struct Maybe<T> : IEnumerable<T>
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
            return this.Any() ? this.Single() : other;
        }

        public Maybe<T> Or(Maybe<T> other)
        {
            return this.Any() ? this : other;
        }

        public T Value
        {
            get { return this.SingleOrDefault(); }
        }


    }


    public static class MaybeExtension
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            if (!typeof (T).IsValueType)
// ReSharper disable once CompareNonConstrainedGenericWithNull
                return value != null ? Maybe<T>.With(value) : Maybe<T>.None;

            return Maybe<T>.With(value);
        }


        public static U IfNotNull<T, U>(this Maybe<T> value, Func<T, U> func)
            where T : class
            where U : class
        {
            if (!value.Any() || value.Single() == null) return null;

            return value.Any()
                ? func(value.Single())
                : null;
        }


        public static Maybe<T> Unit<T>(this T value)
        {
            return Maybe<T>.With(value);
        }


        public static Maybe<V> Bind<U, V>(this Maybe<U> maybe, Func<U, Maybe<V>> func)
        {
            return maybe.Any() ? func(maybe.Value) : Maybe<V>.Nothing;
        }


        public static Func<T, Maybe<V>> Compose<T, U, V>(this Func<U, Maybe<V>> f, Func<T, Maybe<U>> g)
        {
            return x => Bind(g(x), f);
        }
    }
}
