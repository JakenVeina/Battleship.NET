namespace System.Windows
{
    public static class ObservableExtensions
    {
        public static ReactiveProperty<T> ToReactiveProperty<T>(
                this IObservable<T> valueStream)
            => ReactiveProperty.Create(valueStream);

        public static ReactiveProperty<T> ToReactiveProperty<T>(
                this IObservable<T> valueStream,
                T initialValue)
            => ReactiveProperty.Create(valueStream, initialValue);
    }
}
