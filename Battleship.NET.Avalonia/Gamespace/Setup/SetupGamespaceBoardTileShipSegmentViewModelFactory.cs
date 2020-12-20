using System.Drawing;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileShipSegmentViewModelFactory
    {
        public SetupGamespaceBoardTileShipSegmentViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public SetupGamespaceBoardTileShipSegmentViewModel Create(
                Point position,
                int shipIndex)
            => new SetupGamespaceBoardTileShipSegmentViewModel(
                _gameStateStore,
                position,
                shipIndex,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
