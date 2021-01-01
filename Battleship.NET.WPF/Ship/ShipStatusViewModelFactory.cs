using ReduxSharp;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Ship
{
    public class ShipStatusViewModelFactory
    {
        public ShipStatusViewModelFactory(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public ShipStatusViewModel Create(
                GamePlayer player,
                int shipIndex)
            => new ShipStatusViewModel(
                gameStateStore: _gameStateStore,
                player:         player,
                shipIndex:      shipIndex);

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
