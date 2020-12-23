using System;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Player
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
                .ToReactiveProperty();

            IsActive = viewStateStore
                .Select(viewState => (viewState.ActivePlayer == player))
                .ToReactiveProperty();

            Name = model
                .Select(model => model.definition.Name)
                .ToReactiveProperty();

            PlayTime = model
                .Select(model => model.state.PlayTime)
                .ToReactiveProperty();

            Wins = model
                .Select(model => model.state.Wins)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsActive { get; }

        public IReadOnlyObservableProperty<string> Name { get; }

        public IReadOnlyObservableProperty<TimeSpan> PlayTime { get; }

        public IReadOnlyObservableProperty<int> Wins { get; }
    }
}
