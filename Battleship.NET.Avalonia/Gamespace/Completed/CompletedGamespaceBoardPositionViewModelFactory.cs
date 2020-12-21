using System.Drawing;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionViewModelFactory
    {
        public CompletedGamespaceBoardPositionViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public CompletedGamespaceBoardPositionViewModel Create(
                Point position)
            => new CompletedGamespaceBoardPositionViewModel(
                _gameStateStore,
                position,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
