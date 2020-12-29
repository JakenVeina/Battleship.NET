using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceViewModel
    {
        public RunningGamespaceViewModel(
            RunningGamespaceBoardPositionViewModelFactory boardPositionFactory,
            IStore<GameStateModel> gameStateStore)
        {
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

            EndTurnCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new EndTurnAction()),
                canExecute: gameStateStore
                    .Select(gameState => (gameState.CurrentPlayer == GamePlayer.Player1)
                        ? !gameState.Player1.CanFireShot
                        : !gameState.Player2.CanFireShot)
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
