using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceViewModel
    {
        public RunningGamespaceViewModel(
            RunningGamespaceBoardPositionViewModelFactory boardPositionFactory,
            IStore<GameStateModel> gameStateStore)
        {
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .ToReactiveProperty();

            BoardPositions = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                    .ThenBy(position => position.X)
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ToReactiveProperty();

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ToReactiveProperty();

            EndTurnCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new EndTurnAction()),
                canExecute: gameStateStore
                    .Select(gameState => !gameState.CurrentPlayerState.CanFireShot)
                    .DistinctUntilChanged());

            TogglePauseCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()));
        }

        public IReadOnlyObservableProperty<ImmutableArray<RunningGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }

        public ICommand<Unit> EndTurnCommand { get; }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
