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
        : GameBoardViewModelBase<RunningGamespaceBoardPositionViewModel>
    {
        public RunningGamespaceViewModel(
                RunningGamespaceBoardPositionViewModelFactory boardPositionFactory,
                IStore<GameStateModel> gameStateStore)
            : base(
                boardPositionFactory.Create,
                gameStateStore)
        {
            EndTurnCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new EndTurnAction()),
                canExecute: gameStateStore
                    .Select(gameState => (gameState.CurrentPlayer == GamePlayer.Player1)
                        ? gameState.Player1.HasMissed
                        : gameState.Player2.HasMissed)
                    .DistinctUntilChanged());

            TogglePauseCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()));
        }

        public ICommand<Unit> EndTurnCommand { get; }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
