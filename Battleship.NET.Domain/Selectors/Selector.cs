using System;
using System.Collections.Generic;

namespace Battleship.NET.Domain.Selectors
{
    public static class Selector
    {
        public static Func<T, TResult> Memoize<T, TResult>(
            this Func<T, TResult> selector)
        {
            var cache = new MemoizationCache<T, TResult>();

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
            var cache = new MemoizationCache<(T1, T2), TResult>();

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

        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> selector)
        {
            var cache = new MemoizationCache<(T1, T2, T3), TResult>();

            return (arg1, arg2, arg3) =>
            {
                if (!cache.TryGetOutput((arg1, arg2, arg3), out var result))
                {
                    result = selector.Invoke(arg1, arg2, arg3);
                    cache.Update((arg1, arg2, arg3), result);
                }
                return result;
            };
        }

        public static Func<T, TResult> Create<T, TResult>(
                Func<T, TResult> resultSelector)
            => resultSelector.Memoize();

        public static Func<TIn, TOut> Create<TIn, T1, TOut>(
            Func<TIn, T1> argSelector,
            Func<T1, TOut> resultSelector)
        {
            var memoizedResultSelector = resultSelector.Memoize();

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

        public static Func<TIn, TOut> Create<TIn, T1, T2, T3, TOut>(
            Func<TIn, T1> arg1Selector,
            Func<TIn, T2> arg2Selector,
            Func<TIn, T3> arg3Selector,
            Func<T1, T2, T3, TOut> resultSelector)
        {
            var memoizedResultSelector = resultSelector.Memoize();

            return Create<TIn, TOut>(
                input => memoizedResultSelector.Invoke(
                    arg1Selector.Invoke(input),
                    arg2Selector.Invoke(input),
                    arg3Selector.Invoke(input)));
        }
    }
}
