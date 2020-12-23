using System.Collections.Generic;

namespace System
{
    public static class ListExtensions
    {
        public static T PickRandom<T>(
                this IReadOnlyList<T> list,
                Random random)
            => list[random.Next(0, list.Count)];
    }
}
