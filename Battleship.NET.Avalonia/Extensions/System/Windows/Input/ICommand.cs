namespace System.Windows.Input
{
    public interface ICommand<T>
        : ICommand
    {
        bool CanExecute(T parameter);

        void Execute(T parameter);
    }
}
