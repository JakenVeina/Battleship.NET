using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using Redux;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceViewModel
    {
        public SetupGamespaceViewModel(
            IStore<GameStateModel> gameStateStore,
            Random random,
            SetupGamespaceBoardTileViewModelFactory setupGamespaceBoardTileViewModelFactory)
        {
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .DistinctUntilChanged();

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .DistinctUntilChanged();

            BoardTiles = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                        .ThenBy(position => position.X)
                    .Select(position => setupGamespaceBoardTileViewModelFactory.Create(position))
                    .ToImmutableArray())
                .DistinctUntilChanged();

            var currentPlayer = gameStateStore
                .Select(gameState => gameState.CurrentPlayer)
                .DistinctUntilChanged();

            RandomizeShips = ReactiveCommand.Create(
                currentPlayer
                    .Select(currentPlayer => new Action(() => gameStateStore.Dispatch(new RandomizeShipsAction(
                        currentPlayer,
                        random)))));

            CompleteSetup = ReactiveCommand.Create(
                currentPlayer
                    .Select(currentPlayer => new Action(() => gameStateStore.Dispatch(new CompleteSetupAction(
                        currentPlayer)))),
                gameStateStore
                    .Select(gameState => gameState.CanCompleteSetup(gameState.CurrentPlayer))
                    .DistinctUntilChanged());
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileViewModel>> BoardTiles { get; }

        public ICommand<Unit> RandomizeShips { get; }

        public ICommand<Unit> CompleteSetup { get; }
    }
}
