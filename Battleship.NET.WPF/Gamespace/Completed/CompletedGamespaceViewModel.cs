using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;
using Battleship.NET.WPF.State.Actions;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceViewModel
        : GameBoardViewModelBase<CompletedGamespaceBoardPositionViewModel>
    {
        public CompletedGamespaceViewModel(
                CompletedGamespaceBoardPositionViewModelFactory boardPositionFactory,
                IStore<GameStateModel> gameStateStore,
                Random random,
                IStore<ViewStateModel> viewStateStore)
            : base(
                boardPositionFactory.Create,
                gameStateStore)
        {
            BeginSetupCommand = ReactiveCommand.Create(() =>
            {
                gameStateStore.Dispatch(new BeginSetupAction());
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player1, random));
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player2, random));
                viewStateStore.Dispatch(new SetActivePlayerAction(GamePlayer.Player1));
            });

            ToggleActivePlayerCommand = ReactiveCommand.Create(() => viewStateStore.Dispatch(new ToggleActivePlayerAction()));
        }

        public ICommand<Unit> BeginSetupCommand { get; }

        public ICommand<Unit> ToggleActivePlayerCommand { get; }
    }
}
