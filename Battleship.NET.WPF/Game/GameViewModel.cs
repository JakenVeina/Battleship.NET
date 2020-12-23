﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Gamespace.Completed;
using Battleship.NET.WPF.Gamespace.Idle;
using Battleship.NET.WPF.Gamespace.Ready;
using Battleship.NET.WPF.Gamespace.Running;
using Battleship.NET.WPF.Gamespace.Setup;
using Battleship.NET.WPF.Player;

namespace Battleship.NET.WPF.Game
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
                .ToReactiveProperty();

            IsPaused = gameStateStore
                .Select(gameState => gameState.State == GameState.Paused)
                .ToReactiveProperty();

            Player1 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player1);
            Player2 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player2);

            Runtime = gameStateStore
                .Select(gameState => gameState.Runtime)
                .ToReactiveProperty();

            TogglePauseCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new TogglePauseAction()),
                gameStateStore
                    .Select(gameState => (gameState.State == GameState.Paused)
                        || (gameState.State == GameState.Running))
                    .DistinctUntilChanged());
        }

        public IReadOnlyObservableProperty<object?> Gamespace { get; }

        public IReadOnlyObservableProperty<bool> IsPaused { get; }

        public PlayerViewModel Player1 { get; }

        public PlayerViewModel Player2 { get; }

        public IReadOnlyObservableProperty<TimeSpan> Runtime { get; }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}