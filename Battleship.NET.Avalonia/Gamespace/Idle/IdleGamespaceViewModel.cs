using System;
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
            IStore<GameStateModel> gameStateStore,
            Random random)
        {
            BeginSetup = ReactiveCommand.Create(
                () =>
                {
                    gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player1, random));
                    gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player2, random));
                    gameStateStore.Dispatch(new BeginSetupAction());
                });
        }

        public ICommand<Unit> BeginSetup { get; }
    }
}
