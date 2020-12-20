using System.Drawing;

using Redux;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceBoardTileViewModelFactory
    {
        public CompletedGamespaceBoardTileViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public CompletedGamespaceBoardTileViewModel Create(
                Point position)
            => new CompletedGamespaceBoardTileViewModel(
                _gameStateStore,
                position,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
