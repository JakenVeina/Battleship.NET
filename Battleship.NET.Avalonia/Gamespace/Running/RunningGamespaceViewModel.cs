using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows.Input;

using Redux;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceViewModel
    {
        public RunningGamespaceViewModel(
            IStore<GameStateModel> gameStateStore,
            RunningGamespaceBoardTileViewModelFactory runningGamespaceBoardTileViewModelFactory)
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
                    .Select(position => runningGamespaceBoardTileViewModelFactory.Create(position))
                    .ToImmutableArray())
                .DistinctUntilChanged();

            EndTurn = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new EndTurnAction()),
                gameStateStore
                    .Select(gameState => !gameState.CurrentPlayerState.CanFireShot)
                    .DistinctUntilChanged());
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<RunningGamespaceBoardTileViewModel>> BoardTiles { get; }

        public ICommand<Unit> EndTurn { get; }
    }
}
