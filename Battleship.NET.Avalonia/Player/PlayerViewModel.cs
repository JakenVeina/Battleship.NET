using System;
using System.Reactive.Linq;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Player
{
    public class PlayerViewModel
    {
        public PlayerViewModel(
            IStore<GameStateModel> gameStateStore,
            GamePlayer player,
            IStore<ViewStateModel> viewStateStore)
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
                .ShareReplayDistinct(1);

            IsActive = viewStateStore
                .Select(viewState => (viewState.ActivePlayer == player))
                .ShareReplayDistinct(1);

            Name = model
                .Select(model => model.definition.Name)
                .ShareReplayDistinct(1);

            Wins = model
                .Select(model => model.state.Wins)
                .ShareReplayDistinct(1);
        }

        public IObservable<bool> IsActive { get; }

        public IObservable<string> Name { get; }

        public IObservable<int> Wins { get; }
    }
}
