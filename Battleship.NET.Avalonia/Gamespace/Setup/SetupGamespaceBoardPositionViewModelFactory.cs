using System.Drawing;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceBoardPositionViewModelFactory
    {
        public SetupGamespaceBoardPositionViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public SetupGamespaceBoardPositionViewModel Create(
                Point position)
            => new SetupGamespaceBoardPositionViewModel(
                _gameStateStore,
                position,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
