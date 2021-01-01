using System.Collections.Generic;

namespace Battleship.NET.Domain.Selectors
{
    public class MemoizationCache<TIn, TOut>
    {
        public bool TryGetOutput(TIn input, out TOut output)
        {
            if (_cacheEntry.HasValue
                && (EqualityComparer<TIn>.Default.GetHashCode(input) == GetInputHashCode(_cacheEntry.Value.input))
                && EqualityComparer<TIn>.Default.Equals(input, _cacheEntry.Value.input))
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
                _inputHashCode = EqualityComparer<TIn>.Default.GetHashCode(input);
            return _inputHashCode.Value;
        }

        private (TIn input, TOut output)? _cacheEntry;
        private int? _inputHashCode;
    }
}
