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

        public T Or(Func<T> other)
        {
            return this.Any() ? this.Single() : other();
        }

        public Maybe<T> Or(Maybe<T> other)
        {
            return this.Any() ? this : other;
        }

        public T Value
        {
            get { return this.SingleOrDefault(); }
        }

        public bool HasValue
        {
            get
            {
                return !_values.IsNull() && _values.Any();
            }
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


        //public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
        //       where TInput : class
        //{
        //    if (o == null) return null;
        //    return evaluator(o) ? o : null;
        //}

        //public static Maybe<TInput> If<TInput>(this Maybe<TInput> o, Func<TInput, bool> evaluator)
        //{
        //    if (!o.Any()) return o;

        //    return evaluator(o.Single()) ? o : Maybe<TInput>.None;
        //}

        public static TSource If<TSource>(this TSource source, Func<TSource, bool> condition)
            where TSource : class
        {
            if ((source != default(TSource)) && condition(source))
            {
                return source;
            }
            return default(TSource);
        }

        public static Maybe<TSource> If<TSource>(this Maybe<TSource> source, Func<TSource, bool> condition)
        {
            if (source.HasValue && condition(source.Value))
                return source;
            return Maybe<TSource>.None;
        }


        public static TSource IfNull<TSource>(this TSource source, Action func)
            where TSource : class
        {
            if (source == null)
                func();
            return source;
        }

        public static Maybe<TSource> IfNull<TSource>(this Maybe<TSource> source, Action func)
        {
            if (!source.HasValue)
                func();
            return source;
        }


        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
               where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static Maybe<TInput> Unless<TInput>(this Maybe<TInput> o, Func<TInput, bool> evaluator)
        {
            if (!o.Any()) return Maybe<TInput>.None;

            return evaluator(o.Single()) ? Maybe<TInput>.None : o;
        }



        //public static TInput Do<TInput>(this TInput o, Action<TInput> action)
        //    where TInput : class 
        //{
        //    if (o == null) return null;
        //    action(o);
        //    return o;
        //}


        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
        {
            if (!typeof(TInput).IsValueType && o != null)
                action(o);
            return o;
        }

        public static Maybe<TInput> Do<TInput>(this Maybe<TInput> o, Action<TInput> action)
        {
            if (!o.HasValue) return Maybe<TInput>.None;
            action(o.Value);

            return o;
        }


        public static TResult Return<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this Maybe<TInput> o,
            Func<TInput, TResult> evaulator, TResult failureValue)
        {
            return !o.HasValue ? failureValue : evaulator(o.Value);
        }


        public static TResult With<TInput, TResult>(this TInput o,
           Func<TInput, TResult> evaluator)
                where TResult : class
                where TInput : class
        {
            return o == null ? null : evaluator(o);
        }


        public static Maybe<TResult> With<TInput, TResult>(this Maybe<TInput> o,
            Func<TInput, TResult> evaulator)
        {
            return !o.HasValue ? Maybe<TResult>.None : evaulator(o.Value).ToMaybe();
        }

        public static Maybe<TResult> With<TInput, TResult>(this Maybe<TInput> o, Func<TInput, Maybe<TResult>> evalulator)
        {
            return !o.HasValue ? Maybe<TResult>.None : evalulator(o.Value);
        }
    }
}
