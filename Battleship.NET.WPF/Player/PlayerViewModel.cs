using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Player
{
    public class PlayerViewModel
    {
        public PlayerViewModel(
            IStore<GameStateModel> gameStateStore,
            GamePlayer player,
            ShipStatusViewModelFactory shipStatusFactory,
            IStore<ViewStateModel> viewStateStore)
        {
            var playerInfo = ((player == GamePlayer.Player1)
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

            Name = playerInfo
                .Select(playerInfo => playerInfo.definition.Name)
                .ToReactiveProperty();

            PlayTime = playerInfo
                .Select(playerInfo => playerInfo.state.PlayTime)
                .ToReactiveProperty();

            ShipStatuses = gameStateStore
                .Select(gameState => gameState.Definition.Ships.Length)
                .DistinctUntilChanged()
                .Select(shipCount => Enumerable.Range(0, shipCount)
                    .Select(shipIndex => shipStatusFactory.Create(
                        player:     player,
                        shipIndex:  shipIndex))
                    .ToImmutableArray())
                .ToReactiveProperty();

            Wins = playerInfo
                .Select(player => player.state.Wins)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsActive { get; }

        public IReadOnlyObservableProperty<string> Name { get; }

        public IReadOnlyObservableProperty<TimeSpan> PlayTime { get; }

        public IReadOnlyObservableProperty<ImmutableArray<ShipStatusViewModel>> ShipStatuses { get; }

        public IReadOnlyObservableProperty<int> Wins { get; }
    }
}
