using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;

namespace System.Windows
{
    public class ReactiveProperty
    {
        public static ReactiveProperty<T> Create<T>(
                IObservable<T> valueStream)
            => ReactiveProperty<T>.Create(valueStream);

        public static ReactiveProperty<T> Create<T>(
                IObservable<T> valueStream,
                T initialValue)
            => ReactiveProperty<T>.Create(valueStream, initialValue);

        internal static readonly PropertyChangedEventArgs ValueChangedEventArgs
            = new("Value");
    }

    public class ReactiveProperty<T>
        : IReadOnlyObservableProperty<T>
    {
        public static ReactiveProperty<T> Create(
                IObservable<T> valueStream,
                T initialValue)
            => new ReactiveProperty<T>(initialValue, valueStream);

        public static ReactiveProperty<T> Create(
                IObservable<T> valueStream)
            => new ReactiveProperty<T>(default!, valueStream);

        private ReactiveProperty(
            T initialValue,
            IObservable<T> valueStream)
        {
            _subscriptions = new();
            _valueStream = valueStream
                .DistinctUntilChanged()
                .Replay(1)
                .RefCount();

            _value = initialValue;
        }

        public T Value
            => _value;

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                if ((value is not null) && !_subscriptions.ContainsKey(value))
                    _subscriptions.Add(value, _valueStream.Subscribe(x =>
                    {
                        _value = x;
                        value.Invoke(this, ReactiveProperty.ValueChangedEventArgs);
                    }));
            }
            remove
            {
                if ((value is not null) && _subscriptions.TryGetValue(value, out var subscription))
                {
                    _subscriptions.Remove(value);
                    subscription.Dispose();
                }
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
            => _valueStream.Subscribe(observer);

        private readonly Dictionary<PropertyChangedEventHandler, IDisposable> _subscriptions;
        private readonly IObservable<T> _valueStream;

        private T _value;
    }
}
