using Redux;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Player
{
    public class PlayerViewModelFactory
    {
        public PlayerViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public PlayerViewModel CreatePlayerViewModel(
                GamePlayer player)
            => new PlayerViewModel(
                _gameStateStore,
                player,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
