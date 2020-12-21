using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.Gamespace.Completed;
using Battleship.NET.Avalonia.Gamespace.Idle;
using Battleship.NET.Avalonia.Gamespace.Ready;
using Battleship.NET.Avalonia.Gamespace.Running;
using Battleship.NET.Avalonia.Gamespace.Setup;
using Battleship.NET.Avalonia.Player;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Game
{
    public class GameViewModel
    {
        public GameViewModel(
            CompletedGamespaceViewModel completedGamespace,
            IStore<GameStateModel> gameStateStore,
            IdleGamespaceViewModel idleGamespace,
            PlayerViewModelFactory playerViewModelFactory,
            ReadyGamespaceViewModel readyGamespace,
            RunningGamespaceViewModel runningGamespace,
            SetupGamespaceViewModel setupGamespace)
        {
            Gamespace = gameStateStore
                .Select(gameState => gameState.State)
                .DistinctUntilChanged()
                .Select(state => (object)(state switch
                {
                    GameState.Complete  => completedGamespace,
                    GameState.Idle      => idleGamespace,
                    GameState.Paused    => null,
                    GameState.Ready     => readyGamespace,
                    GameState.Running   => runningGamespace,
                    GameState.Setup     => setupGamespace,
                    _                   => throw new InvalidOperationException("Dafuq did you do to the game state?"),
                }))
                .ShareReplayDistinct(1);

            IsPaused = gameStateStore
                .Select(gameState => gameState.State == GameState.Paused)
                .ShareReplayDistinct(1);

            Player1 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player1);
            Player2 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player2);

            Runtime = gameStateStore
                .Select(gameState => gameState.Runtime)
                .ShareReplayDistinct(1);

            TogglePauseCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()),
                gameStateStore
                    .Select(gameState => (gameState.State == GameState.Paused)
                        || (gameState.State == GameState.Running))
                    .DistinctUntilChanged());
        }

        public IObservable<object?> Gamespace { get; }

        public IObservable<bool> IsPaused { get; }

        public PlayerViewModel Player1 { get; }

        public PlayerViewModel Player2 { get; }

        public IObservable<TimeSpan> Runtime { get; }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
