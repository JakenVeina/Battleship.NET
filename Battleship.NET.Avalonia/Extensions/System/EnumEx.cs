using System.Collections.Generic;

namespace System
{
    public static class EnumEx
    {
        public static IReadOnlyList<T> GetValues<T>()
                where T : struct, IConvertible
            => (T[])Enum.GetValues(typeof(T));
    }
}
