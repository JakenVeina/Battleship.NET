using System.Linq;

namespace System.Collections.Generic
{
    public class SequenceEqualityComparer<T>
        : IEqualityComparer<IReadOnlyList<T>>
    {
        public static readonly SequenceEqualityComparer<T> Default
            = new SequenceEqualityComparer<T>();

        public bool Equals(IReadOnlyList<T> x, IReadOnlyList<T> y)
        {
            if (x.Count != y.Count)
                return false;

            foreach(var pair in Enumerable.Zip(x, y, (x, y) => (x, y)))
                if (!EqualityComparer<T>.Default.Equals(pair.x, pair.y))
                    return false;

            return true;
        }

        public int GetHashCode(IReadOnlyList<T> obj)
        {
            var hashCode = new HashCode();

            foreach (var item in obj)
                hashCode.Add(item?.GetHashCode() ?? 0);

            return hashCode.ToHashCode();
        }
    }
}
