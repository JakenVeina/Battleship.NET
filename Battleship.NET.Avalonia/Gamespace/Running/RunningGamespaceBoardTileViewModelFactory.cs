using System.Drawing;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceBoardTileViewModelFactory
    {
        public RunningGamespaceBoardTileViewModelFactory(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public RunningGamespaceBoardTileViewModel Create(
                Point position)
            => new RunningGamespaceBoardTileViewModel(
                _gameStateStore,
                position);

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
