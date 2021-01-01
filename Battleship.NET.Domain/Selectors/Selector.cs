using System;
using System.Collections.Generic;

namespace Battleship.NET.Domain.Selectors
{
    public static class Selector
    {
        public static Func<T, TResult> Memoize<T, TResult>(
            this Func<T, TResult> selector,
            IEqualityComparer<T>? argComparer = null)
        {
            var cache = new MemoizationCache<T, TResult>(argComparer ?? EqualityComparer<T>.Default);

            return arg =>
            {
                if (!cache.TryGetOutput(arg, out var result))
                {
                    result = selector.Invoke(arg);
                    cache.Update(arg, result);
                }
                return result;
            };
        }

        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(
            this Func<T1, T2, TResult> selector)
        {
            var cache = new MemoizationCache<(T1, T2), TResult>(EqualityComparer<(T1, T2)>.Default);

            return (arg1, arg2) =>
            {
                if (!cache.TryGetOutput((arg1, arg2), out var result))
                {
                    result = selector.Invoke(arg1, arg2);
                    cache.Update((arg1, arg2), result);
                }
                return result;
            };
        }

        public static Func<T, TResult> Create<T, TResult>(
                Func<T, TResult> resultSelector,
                IEqualityComparer<T>? argComparer = null)
            => resultSelector.Memoize(argComparer ?? EqualityComparer<T>.Default);

        public static Func<TIn, TOut> Create<TIn, T1, TOut>(
            Func<TIn, T1> argSelector,
            Func<T1, TOut> resultSelector,
            IEqualityComparer<T1>? argComparer = null)
        {
            var memoizedResultSelector = resultSelector.Memoize(argComparer ?? EqualityComparer<T1>.Default);

            return Create<TIn, TOut>(
                input => memoizedResultSelector.Invoke(
                    argSelector.Invoke(input)));
        }

        public static Func<TIn, TOut> Create<TIn, T1, T2, TOut>(
            Func<TIn, T1> arg1Selector,
            Func<TIn, T2> arg2Selector,
            Func<T1, T2, TOut> resultSelector)
        {
            var memoizedResultSelector = resultSelector.Memoize();

            return Create<TIn, TOut>(
                input => memoizedResultSelector.Invoke(
                    arg1Selector.Invoke(input),
                    arg2Selector.Invoke(input)));
        }
    }
}
