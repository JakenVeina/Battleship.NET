namespace System.Reactive.Linq
{
    public static class ObservableEx
    {
        public static IObservable<T> ShareReplayDistinct<T>(this IObservable<T> source, int bufferSize)
            => source
                .DistinctUntilChanged()
                .Replay(bufferSize)
                .RefCount();
    }
}
