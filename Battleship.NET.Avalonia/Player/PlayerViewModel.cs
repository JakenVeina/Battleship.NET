using System;
using System.Reactive.Linq;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Player
{
    public class PlayerViewModel
    {
        public PlayerViewModel(
            IStore<GameStateModel> gameStateStore,
            GamePlayer player)
        {
            var model = ((player == GamePlayer.Player1)
                    ? gameStateStore.Select(gameState => 
                    (
                        definition: gameState.Definition.Player1,
                        state:      gameState.Player1
                    ))
                    : gameStateStore.Select(gameState =>
                    (
                        definition: gameState.Definition.Player2,
                        state:      gameState.Player2
                    )))
                .DistinctUntilChanged();

            IsCurrent = gameStateStore
                .Select(gameState => (gameState.CurrentPlayer == player)
                    && ((gameState.State == GameState.Setup)
                        || (gameState.State == GameState.Running)))
                .DistinctUntilChanged();

            Name = model
                .Select(model => model.definition.Name)
                .DistinctUntilChanged();

            Wins = model
                .Select(model => model.state.Wins)
                .DistinctUntilChanged();
        }

        public IObservable<bool> IsCurrent { get; }

        public IObservable<string> Name { get; }

        public IObservable<int> Wins { get; }
    }
}
