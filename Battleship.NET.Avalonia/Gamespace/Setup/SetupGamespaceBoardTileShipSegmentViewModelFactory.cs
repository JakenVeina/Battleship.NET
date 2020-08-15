using System.Drawing;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileShipSegmentViewModelFactory
    {
        public SetupGamespaceBoardTileShipSegmentViewModelFactory(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public SetupGamespaceBoardTileShipSegmentViewModel Create(
                Point position,
                int shipIndex)
            => new SetupGamespaceBoardTileShipSegmentViewModel(
                _gameStateStore,
                position,
                shipIndex);

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
