using System;
using System.Reactive.Linq;

using Redux;

using Battleship.NET.Avalonia.Gamespace.Idle;
using Battleship.NET.Avalonia.Gamespace.Paused;
using Battleship.NET.Avalonia.Gamespace.Ready;
using Battleship.NET.Avalonia.Gamespace.Setup;
using Battleship.NET.Avalonia.Player;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Game
{
    public class GameViewModel
    {
        public GameViewModel(
            IStore<GameStateModel> gameStateStore,
            IdleGamespaceViewModel idleGamespace,
            PausedGamespaceViewModel pausedGamespace,
            PlayerViewModelFactory playerViewModelFactory,
            ReadyGamespaceViewModel readyGamespace,
            SetupGamespaceViewModel setupGamespace)
        {
            Gamespace = gameStateStore
                .Select(gameState => gameState.State)
                .DistinctUntilChanged()
                .Select(state => (object)(state switch
                {
                    GameState.Complete  => throw new NotImplementedException(),
                    GameState.Idle      => idleGamespace,
                    GameState.Paused    => pausedGamespace,
                    GameState.Ready     => readyGamespace,
                    GameState.Running   => throw new NotImplementedException(),
                    GameState.Setup     => setupGamespace,
                    _                   => throw new InvalidOperationException("Dafuq did you do to the game state?"),
                }))
                .DistinctUntilChanged();

            Player1 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player1);
            Player2 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player2);
        }

        public IObservable<object> Gamespace { get; }

        public PlayerViewModel Player1 { get; }

        public PlayerViewModel Player2 { get; }
    }
}
