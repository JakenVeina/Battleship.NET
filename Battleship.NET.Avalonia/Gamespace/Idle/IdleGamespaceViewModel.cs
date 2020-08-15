using System.Reactive;
using System.Windows.Input;

using Redux;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Idle
{
    public class IdleGamespaceViewModel
    {
        public IdleGamespaceViewModel(
            IStore<GameStateModel> gameStateStore)
        {
            BeginSetup = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new BeginSetupAction()));
        }

        public ICommand<Unit> BeginSetup { get; }
    }
}
