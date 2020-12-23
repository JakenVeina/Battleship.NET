using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using ReduxSharp;

using Battleship.NET.Domain.Behaviors;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.State.Actions;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.State.Behaviors
{
    public class ActivePlayerSynchronizationBehavior
        : IBehavior
    {
        public ActivePlayerSynchronizationBehavior(
            IStore<GameStateModel> gameStateStore,
            IStore<ViewStateModel> viewStateStore)
        {
            _gameStateStore = gameStateStore;
            _viewStateStore = viewStateStore;
        }

        public IDisposable Start(IScheduler scheduler)
            => _gameStateStore
                .Select(gameState =>
                (
                    currentPlayer: gameState.CurrentPlayer,
                    state: gameState.State
                ))
                .DistinctUntilChanged()
                .Where(model => (model.state != GameState.Setup)
                    && (model.state != GameState.Complete))
                .Do(model => _viewStateStore.Dispatch(new SetActivePlayerAction(model.state switch
                    {
                        GameState.Running   => model.currentPlayer,
                        _                   => null
                    })))
                .Subscribe();

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly IStore<ViewStateModel> _viewStateStore;
    }
}
