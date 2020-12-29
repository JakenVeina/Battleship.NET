using System.Collections.Generic;

namespace Battleship.NET.Domain.Selectors
{
    public class MemoizationCache<TIn, TOut>
    {
        public MemoizationCache(IEqualityComparer<TIn> inputComparer)
        {
            _inputComparer = inputComparer;
        }

        public bool TryGetOutput(TIn input, out TOut output)
        {
            if (_cacheEntry.HasValue
                && (_inputComparer.GetHashCode(input) == GetInputHashCode(_cacheEntry.Value.input))
                && _inputComparer.Equals(input, _cacheEntry.Value.input))
            {
                output = _cacheEntry.Value.output;
                return true;
            }
            else
            {
                output = default!;
                return false;
            }
        }

        public void Update(TIn input, TOut output)
        {
            _cacheEntry = (input, output);
            _inputHashCode = null;
        }

        private int GetInputHashCode(TIn input)
        {
            if (!_inputHashCode.HasValue)
                _inputHashCode = _inputComparer.GetHashCode(input);
            return _inputHashCode.Value;
        }

        private readonly IEqualityComparer<TIn> _inputComparer;

        private (TIn input, TOut output)? _cacheEntry;
        private int? _inputHashCode;
    }
}
