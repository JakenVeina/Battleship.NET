using System.Drawing;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileViewModelFactory
    {
        public SetupGamespaceBoardTileViewModelFactory(
            SetupGamespaceBoardTileShipSegmentViewModelFactory gameBoardTileShipSegmentViewModelFactory,
            IStore<GameStateModel> gameStateStore)
        {
            _gameBoardTileShipSegmentViewModelFactory = gameBoardTileShipSegmentViewModelFactory;
            _gameStateStore = gameStateStore;
        }

        public SetupGamespaceBoardTileViewModel Create(
                Point position)
            => new SetupGamespaceBoardTileViewModel(
                _gameBoardTileShipSegmentViewModelFactory,
                _gameStateStore,
                position);

        private readonly SetupGamespaceBoardTileShipSegmentViewModelFactory _gameBoardTileShipSegmentViewModelFactory;
        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
