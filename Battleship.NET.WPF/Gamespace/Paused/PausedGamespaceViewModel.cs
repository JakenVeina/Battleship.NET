using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Gamespace.Paused
{
    public class PausedGamespaceViewModel
    {
        public PausedGamespaceViewModel(
            IStore<GameStateModel> gameStateStore)
        {
            TogglePauseCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()));
        }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
