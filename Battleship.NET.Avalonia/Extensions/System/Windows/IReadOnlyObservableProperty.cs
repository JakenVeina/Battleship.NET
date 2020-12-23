using System;
using System.ComponentModel;

namespace System.Windows
{
    public interface IReadOnlyObservableProperty<T>
        : INotifyPropertyChanged,
            IObservable<T>
    {
        T Value { get; }
    }
}
