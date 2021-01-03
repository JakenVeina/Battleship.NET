using System.Reactive;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Idle
{
    public class IdleGamespaceViewModel
    {
        public IdleGamespaceViewModel()
        {
            // TODO: Implement this
            BeginSetupCommand = ReactiveCommand.Create(() => { });
        }

        public ICommand<Unit> BeginSetupCommand { get; }
    }
}
