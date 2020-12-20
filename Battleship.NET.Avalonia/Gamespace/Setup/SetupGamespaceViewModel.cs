using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Actions;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceViewModel
    {
        public SetupGamespaceViewModel(
            SetupGamespaceBoardTileViewModelFactory boardTileFactory,
            IStore<GameStateModel> gameStateStore,
            Random random,
            IStore<ViewStateModel> viewStateStore)
        {
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .ShareReplayDistinct(1);

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ShareReplayDistinct(1);

            BoardTiles = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                        .ThenBy(position => position.X)
                    .Select(position => boardTileFactory.Create(position))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);

            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .Where(activePlayer => activePlayer.HasValue)
                .Select(activePlayer => activePlayer!.Value)
                .ShareReplayDistinct(1);

            RandomizeShips = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() => gameStateStore.Dispatch(new RandomizeShipsAction(
                        activePlayer,
                        random)))));

            CompleteSetup = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() =>
                    {
                        gameStateStore.Dispatch(new CompleteSetupAction(activePlayer));
                        viewStateStore.Dispatch(new ToggleActivePlayerAction());
                    })),
                Observable.CombineLatest(
                        gameStateStore,
                        activePlayer,
                        (gameState, activePlayer) => gameState.CanCompleteSetup(activePlayer))
                    .DistinctUntilChanged());
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileViewModel>> BoardTiles { get; }

        public ICommand<Unit> RandomizeShips { get; }

        public ICommand<Unit> CompleteSetup { get; }
    }
}
