using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static ReadOnlyValueList<T> ToReadOnlyValueList<T>(this IReadOnlyList<T> list)
            => new ReadOnlyValueList<T>(list);

        public static ReadOnlyValueList<T> ToReadOnlyValueList<T>(this IEnumerable<T> sequence)
            => new ReadOnlyValueList<T>(sequence.ToArray());
    }
}
