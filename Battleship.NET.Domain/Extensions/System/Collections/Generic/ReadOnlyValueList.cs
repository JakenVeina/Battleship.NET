using System.Linq;

namespace System.Collections.Generic
{
    public struct ReadOnlyValueList<T>
        : IReadOnlyList<T>, IEquatable<ReadOnlyValueList<T>>
    {
        public ReadOnlyValueList(
            IReadOnlyList<T> values)
        {
            _hashCode = null;
            _values = values;
        }

        public T this[int index]
            => _values[index];

        public int Count
            => _values.Count;

        public bool Equals(ReadOnlyValueList<T> other)
        {
            if (_values == other._values)
                return true;

            if (_values.Count != other._values.Count)
                return false;

            foreach (var pair in Enumerable.Zip(_values, other._values, (x, y) => (x, y)))
                if (!EqualityComparer<T>.Default.Equals(pair.x, pair.y))
                    return false;

            return true;
        }

        public override bool Equals(object obj)
            => (obj is ReadOnlyValueList<T> other)
                && Equals(other);

        public IEnumerator<T> GetEnumerator()
            => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _values.GetEnumerator();

        public override int GetHashCode()
        {
            if (!_hashCode.HasValue)
            {
                var hashCode = new HashCode();

                foreach (var item in _values)
                    hashCode.Add(item?.GetHashCode() ?? 0);

                _hashCode = hashCode.ToHashCode();
            }

            return _hashCode.Value;
        }

        private readonly IReadOnlyList<T> _values;

        private int? _hashCode;
    }
}
