using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Paused
{
    public class PausedGamespaceViewModel
    {
        public PausedGamespaceViewModel(
            IStore<GameStateModel> gameStateStore)
        {
            Resume = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()));
        }

        public ICommand<Unit> Resume { get; }
    }
}
