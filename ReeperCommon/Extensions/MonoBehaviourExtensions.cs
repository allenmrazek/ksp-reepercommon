using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using ReeperCommon.Containers;
using ReeperCommon.Utilities;
using UnityEngine;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
namespace ReeperCommon.Extensions
{
    public static class MonoBehaviourExtensions
    {
        private static Coroutine<T> StartCoroutine_Internal<T>(
            [NotNull] MonoBehaviour host,
            [NotNull] IEnumerator routine, 
            Maybe<T> defaultValue)
        {
            if (host == null) throw new ArgumentNullException("host");
            if (routine == null) throw new ArgumentNullException("routine");
            var wrappedRoutine = defaultValue.Any() ? new Coroutine<T>(defaultValue.Value) : new Coroutine<T>();

            var running = host.StartCoroutine(wrappedRoutine.Run(routine));
            wrappedRoutine.YieldUntilComplete = running;

            return wrappedRoutine;
        }


        private static Coroutine<ValuelessCoroutine> StartCoroutineValueless_Internal(
            [NotNull] MonoBehaviour host,
            [NotNull] IEnumerator routine)
        {
            if (host == null) throw new ArgumentNullException("host");
            if (routine == null) throw new ArgumentNullException("routine");
            var wrappedRoutine = new Coroutine<ValuelessCoroutine>();

            var running = host.StartCoroutine(wrappedRoutine.Run(routine));
            wrappedRoutine.YieldUntilComplete = running;

            return wrappedRoutine;
        }


        public static Coroutine<T> StartCoroutine<T>(this MonoBehaviour host, IEnumerator routine)
        {
            return StartCoroutine_Internal(host, routine, Maybe<T>.None);
        }


        public static Coroutine<T> StartCoroutine<T>(this MonoBehaviour host, IEnumerator routine, T defaultValue)
        {
            return StartCoroutine_Internal(host, routine, defaultValue.ToMaybe());
        }


        public static Coroutine<ValuelessCoroutine> StartCoroutineValueless(this MonoBehaviour host, IEnumerator routine)
        {
            return StartCoroutineValueless_Internal(host, routine);
        }


        public static Coroutine<T> StartCoroutine<T>(this CoroutineHoster host, IEnumerator routine)
        {
            return StartCoroutine_Internal(host, routine, Maybe<T>.None);
        }


        public static Coroutine<T> StartCoroutine<T>(this CoroutineHoster host, IEnumerator routine, T defaultValue)
        {
            return StartCoroutine_Internal(host, routine, defaultValue.ToMaybe());
        }
    }


    // ReSharper disable once ClassNeverInstantiated.Global
    public class Coroutine<T> // todo: CustomYieldInstruction once 5.3 is available
    {
        private readonly Type _desiredType = typeof(T);
        private Maybe<T> _returnValue;
        private Maybe<Exception> _exception = Maybe<Exception>.None;

        public Coroutine YieldUntilComplete;

        // ReSharper disable once MemberCanBePrivate.Global
        public virtual T Value
        {
            get
            {
                // ReSharper disable once ThrowingSystemException
                if (_exception.Any()) throw _exception.Value;
                if (_returnValue.Any()) return _returnValue.Value;

                // ReSharper disable once ThrowingSystemException
                throw new Exception(
                    "Return value was not set. Check the coroutine and ensure it returns a value of the type expected by the caller (or throws).");
            }
        }


        public bool HasValue
        {
            get { return _returnValue.Any(); }
        }


        public Maybe<Exception> Error
        {
            get
            {
                return _exception;
            }
        }


        public Coroutine(T defaultValue)
        {
            _returnValue = Maybe<T>.With(defaultValue);
        }


        public Coroutine()
        {
            _returnValue = Maybe<T>.None;
        }


        public IEnumerator Run([NotNull] IEnumerator userRoutine)
        {
            if (userRoutine == null) throw new ArgumentNullException("userRoutine");

            while (true)
            {
                try
                {
                    if (!userRoutine.MoveNext())
                        yield break;
                }
                    // ReSharper disable once CatchAllClause
                catch (Exception e)
                {
                    _exception = e.ToMaybe();
                    yield break;
                }

                var yielded = userRoutine.Current;

                var yieldedType = yielded.With(y => y.GetType());

                if (yieldedType == _desiredType)
                {
                    _returnValue = ((T)yielded).ToMaybe();
                    yield break;
                } /*else if (yieldedType.IsGenericType && yieldedType.GetGenericTypeDefinition() == typeof (Coroutine<>))
                {
                    var coroutine = yieldedType.GetField("YieldUntilComplete", BindingFlags.Instance | BindingFlags.Public)
                        .IfNull(
                            () => { throw new MissingFieldException("Missing expected field", "YieldUntilComplete"); })
                        .With(fi => (Coroutine) fi.GetValue(yielded));

                    var errorProperty

                    yield return coroutine; // wait for nested coroutine to finish

                }*/
                else
                {

                    yield return userRoutine.Current;
                }
            }
        }


        //public IEnumerator Run<TU>([NotNull] Coroutine<TU> customRoutine, IEnumerator userRoutine)
        //{
        //    IEnumerator innerRoutine = null;

        //    try
        //    {
        //        innerRoutine = customRoutine.Run(userRoutine);
        //    }
        //    catch (Exception e)
        //    {
        //        _exception = e.ToMaybe();
        //        yield break;
        //    }


        //    while (true)
        //    {
        //        try
        //        {
        //            if (!innerRoutine.MoveNext())
        //                yield break;
        //        }
        //        // ReSharper disable once CatchAllClause
        //        catch (Exception e)
        //        {
        //            _exception = e.ToMaybe();
        //            yield break;
        //        }

        //        var yielded = innerRoutine.Current;

        //        if (yielded.With(y => y.GetType()) == _desiredType)
        //        {
        //            _returnValue = ((T)yielded).ToMaybe();
        //            yield break;
        //        }

        //        yield return innerRoutine.Current;
        //    }
        //}
    }

    public struct CoroutineWithoutReturnValue
    {
        
    }

    public class ValuelessCoroutine : Coroutine<CoroutineWithoutReturnValue>
    {
        public override CoroutineWithoutReturnValue Value
        {
            get { throw new InvalidOperationException("This coroutine does not return a value."); }
        }
    }
}
