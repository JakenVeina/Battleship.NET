using System.Drawing;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileViewModelFactory
    {
        public SetupGamespaceBoardTileViewModelFactory(
            SetupGamespaceBoardTileShipSegmentViewModelFactory gameBoardTileShipSegmentViewModelFactory,
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameBoardTileShipSegmentViewModelFactory = gameBoardTileShipSegmentViewModelFactory;
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public SetupGamespaceBoardTileViewModel Create(
                Point position)
            => new SetupGamespaceBoardTileViewModel(
                _gameBoardTileShipSegmentViewModelFactory,
                _gameStateStore,
                position,
                _viewStateStore);

        private readonly SetupGamespaceBoardTileShipSegmentViewModelFactory _gameBoardTileShipSegmentViewModelFactory;
        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
