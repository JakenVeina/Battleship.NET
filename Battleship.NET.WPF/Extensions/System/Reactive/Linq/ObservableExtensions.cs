namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> ShareReplay<T>(this IObservable<T> source, int bufferSize)
            => source
                .Replay(bufferSize)
                .RefCount();

        public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source)
                where T : struct
            => source
                .Where(value => value.HasValue)
                .Select(value => value!.Value);
    }
}
