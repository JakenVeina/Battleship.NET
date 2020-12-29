using System;
using System.Collections.Generic;

namespace Battleship.NET.Domain.Selectors
{
    public static class SelectorSet
    {
        public static SelectorSet<TIn, TOut, TParams> Create<TIn, TOut, TParams>(
                Func<TParams, Func<TIn, TOut>> selectorFactory)
            => new SelectorSet<TIn, TOut, TParams>(selectorFactory);
    }

    public class SelectorSet<TIn, TOut, TParams>
    {
        public SelectorSet(Func<TParams, Func<TIn, TOut>> selectorFactory)
        {
            _selectorFactory = selectorFactory;
            _selectorsByParams = new();
        }

        public Func<TIn, TOut> this[TParams @params]
        {
            get
            {
                if (!_selectorsByParams.TryGetValue(@params, out var selector))
                {
                    selector = _selectorFactory.Invoke(@params);
                    _selectorsByParams.Add(@params, selector);
                }

                return selector;
            }
        }

        private readonly Func<TParams, Func<TIn, TOut>> _selectorFactory;
        private readonly Dictionary<TParams, Func<TIn, TOut>> _selectorsByParams;
    }
}
