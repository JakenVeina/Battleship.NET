using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Player
{
    public class PlayerViewModelFactory
    {
        public PlayerViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            ShipStatusViewModelFactory shipStatusFactory,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _shipStatusFactory = shipStatusFactory;
            _viewStateStore = viewStateStore;
        }

        public PlayerViewModel CreatePlayerViewModel(
                GamePlayer player)
            => new PlayerViewModel(
                _gameStateStore,
                player,
                _shipStatusFactory,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly ShipStatusViewModelFactory _shipStatusFactory;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
