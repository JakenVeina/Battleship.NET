using System.Drawing;

using ReduxSharp;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceBoardPositionViewModelFactory
    {
        public RunningGamespaceBoardPositionViewModelFactory(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public RunningGamespaceBoardPositionViewModel Create(
                Point position)
            => new RunningGamespaceBoardPositionViewModel(
                _gameStateStore,
                position);

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
