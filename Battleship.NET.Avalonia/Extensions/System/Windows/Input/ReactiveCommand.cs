using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace System.Windows.Input
{
    public static class ReactiveCommand
    {
        public static ReactiveCommand<Unit> Create(
                Action execute,
                IObservable<bool>? canExecute = null)
            => Create(
                Observable.Return(execute),
                canExecute);

        public static ReactiveCommand<Unit> Create(
                IObservable<Action> execute,
                IObservable<bool>? canExecute = null)
            => new ReactiveCommand<Unit>(
                execute.Select(execute => new Action<Unit>(_ => execute.Invoke())),
                canExecute?.Select(canExecute => canExecute
                        ? ReactiveCommand<Unit>.True
                        : ReactiveCommand<Unit>.False)
                    ?? Observable.Return(ReactiveCommand<Unit>.True),
                ReactiveCommand<Unit>.UseDefault);

        public static ReactiveCommand<T> Create<T>(
                IObservable<Action<T>> execute,
                IObservable<bool>? canExecute = null)
            => new ReactiveCommand<T>(
                execute,
                canExecute?.Select(canExecute => canExecute
                        ? ReactiveCommand<T>.True
                        : ReactiveCommand<T>.False)
                    ?? Observable.Return(ReactiveCommand<T>.True),
                ReactiveCommand<T>.CastParameter);
    }

    public sealed class ReactiveCommand<T>
        : ICommand<T>,
            IDisposable
    {
        public ReactiveCommand(
            IObservable<Action<T>> execute,
            IObservable<Predicate<T>> canExecute,
            Func<object?, T> parameterConverter)
        {
            _execute = Nop;
            _canExecute = False;
            _parameterConverter = parameterConverter;

            CanExecuteChanged = (_, _) => { };

            _executeSubscription = execute
                .Subscribe(execute => _execute = execute);

            _canExecuteSubscription = canExecute
                ?.Subscribe(canExecute =>
                {
                    _canExecute = canExecute;
                    CanExecuteChanged.Invoke(this, EventArgs.Empty);
                });
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(T parameter)
            => _canExecute.Invoke(parameter);

        bool ICommand.CanExecute(object? parameter)
            => _canExecute.Invoke(_parameterConverter.Invoke(parameter));

        public void Dispose()
        {
            _canExecuteSubscription?.Dispose();
            _executeSubscription.Dispose();
        }

        public void Execute(T parameter)
            => _execute.Invoke(parameter);

        void ICommand.Execute(object? parameter)
            => _execute.Invoke(_parameterConverter.Invoke(parameter));

        private readonly IDisposable? _canExecuteSubscription;
        private readonly IDisposable _executeSubscription;
        private readonly Func<object?, T> _parameterConverter;

        private Predicate<T> _canExecute;
        private Action<T> _execute;

        internal static readonly Func<object?, T> CastParameter
            = (parameter => (T)parameter!);

        internal static readonly Predicate<T> False
            = (_ => false);

        internal static readonly Action<T> Nop
            = (_ => { });

        internal static readonly Predicate<T> True
            = (_ => true);

        internal static readonly Func<object?, T> UseDefault
            = (_ => default!);
    }
}
