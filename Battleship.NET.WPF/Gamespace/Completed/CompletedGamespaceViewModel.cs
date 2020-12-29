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
    {
        public CompletedGamespaceViewModel(
            CompletedGamespaceBoardPositionViewModelFactory boardPositionFactory,
            IStore<GameStateModel> gameStateStore,
            Random random,
            IStore<ViewStateModel> viewStateStore)
        {
            BeginSetupCommand = ReactiveCommand.Create(() =>
            {
                gameStateStore.Dispatch(new BeginSetupAction());
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player1, random));
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player2, random));
                viewStateStore.Dispatch(new SetActivePlayerAction(GamePlayer.Player1));
            });

            BoardPositions = gameStateStore
                .Select(gameState => gameState.Definition)
                .DistinctUntilChanged()
                .Select(definition => definition.GameBoard.Positions
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ToReactiveProperty();

            BoardSize = gameStateStore
                .Select(BoardSelectors.Size)
                .ToReactiveProperty();

            ToggleActivePlayerCommand = ReactiveCommand.Create(() => viewStateStore.Dispatch(new ToggleActivePlayerAction()));
        }

        public ICommand<Unit> BeginSetupCommand { get; }

        public IReadOnlyObservableProperty<ImmutableArray<CompletedGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }

        public ICommand<Unit> ToggleActivePlayerCommand { get; }
    }
}
