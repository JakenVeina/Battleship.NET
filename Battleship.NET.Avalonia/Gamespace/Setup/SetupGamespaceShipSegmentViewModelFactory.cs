using System.Drawing;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentViewModelFactory
    {
        public SetupGamespaceShipSegmentViewModelFactory(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public SetupGamespaceShipSegmentViewModel Create(
                Point segment,
                int shipIndex)
            => new SetupGamespaceShipSegmentViewModel(
                _gameStateStore,
                segment,
                shipIndex,
                _viewStateStore);

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
