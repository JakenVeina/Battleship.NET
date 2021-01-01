using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Ship
{
    public class ShipStatusViewModel
    {
        public ShipStatusViewModel(
            IStore<GameStateModel> gameStateStore,
            GamePlayer player,
            int shipIndex)
        {
            IsSunk = gameStateStore
                .Select(ShipSelectors.IsSunk[(player, shipIndex)])
                .ToReactiveProperty();

            Name = gameStateStore
                .Select(gameState => gameState.Definition.Ships[shipIndex].Name)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsSunk { get; }

        public IReadOnlyObservableProperty<string> Name { get; }
    }
}
