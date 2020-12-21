using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceViewModel
    {
        public RunningGamespaceViewModel(
            RunningGamespaceBoardPositionViewModelFactory boardPositionFactory,
            IStore<GameStateModel> gameStateStore)
        {
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .ShareReplayDistinct(1);

            BoardPositions = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                    .ThenBy(position => position.X)
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ShareReplayDistinct(1);

            EndTurnCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new EndTurnAction()),
                canExecute: gameStateStore
                    .Select(gameState => !gameState.CurrentPlayerState.CanFireShot)
                    .DistinctUntilChanged());
        }

        public IObservable<ImmutableArray<RunningGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IObservable<Size> BoardSize { get; }

        public ICommand<Unit> EndTurnCommand { get; }
    }
}
