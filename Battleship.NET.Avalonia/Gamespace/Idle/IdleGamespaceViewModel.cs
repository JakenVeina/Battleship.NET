using System;
using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Actions;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Idle
{
    public class IdleGamespaceViewModel
    {
        public IdleGamespaceViewModel(
            IStore<GameStateModel> gameStateStore,
            Random random,
            IStore<ViewStateModel> viewStateStore)
        {
            BeginSetup = ReactiveCommand.Create(
                () =>
                {
                    gameStateStore.Dispatch(new BeginSetupAction());
                    gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player1, random));
                    gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player2, random));
                    viewStateStore.Dispatch(new SetActivePlayerAction(GamePlayer.Player1));
                });
        }

        public ICommand<Unit> BeginSetup { get; }
    }
}
