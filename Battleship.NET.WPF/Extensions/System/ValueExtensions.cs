namespace System
{
    public static class ValueExtensions
    {
        public static T? ToNullable<T>(this T value)
                where T : struct
            => new T?(value);
    }
}
