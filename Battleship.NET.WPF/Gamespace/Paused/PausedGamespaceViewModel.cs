using System.Reactive;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Paused
{
    public class PausedGamespaceViewModel
    {
        public PausedGamespaceViewModel()
        {
            // TODO: Implement this
            TogglePauseCommand = ReactiveCommand.Create(() => { });
        }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
