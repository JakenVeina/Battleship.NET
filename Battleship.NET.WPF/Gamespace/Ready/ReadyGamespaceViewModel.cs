using System.Reactive;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Ready
{
    public class ReadyGamespaceViewModel
    {
        public ReadyGamespaceViewModel()
        {
            // TODO: Implement this
            StartGameCommand = ReactiveCommand.Create(() => { });
        }

        public ICommand<Unit> StartGameCommand { get; }
    }
}
