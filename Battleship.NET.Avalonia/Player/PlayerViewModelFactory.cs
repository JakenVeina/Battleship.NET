using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Player
{
    public class PlayerViewModelFactory
    {
        public PlayerViewModelFactory(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public PlayerViewModel CreatePlayerViewModel(
                GamePlayer player)
            => new PlayerViewModel(
                _gameStateStore,
                player);

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
