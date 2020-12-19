using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Redux;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Behaviors
{
    public class GameCompletionBehavior
        : IBehavior
    {
        public GameCompletionBehavior(
            IStore<GameStateModel> gameStateStore)
        {
            _gameStateStore = gameStateStore;
        }

        public IDisposable Start(IScheduler scheduler)
            => _gameStateStore
                .ObserveOn(scheduler)
                .Select(gameState => 
                (
                    maxHitCount:        gameState.Definition.Ships.Sum(ship => ship.Segments.Count),
                    player1HitCount:    gameState.Player1.GameBoard.Hits.Count,
                    player2HitCount:    gameState.Player2.GameBoard.Hits.Count
                ))
                .DistinctUntilChanged()
                .Do(model =>
                {
                    if (model.player1HitCount >= model.maxHitCount)
                        _gameStateStore.Dispatch(new CompleteGameAction(GamePlayer.Player1));
                    else if (model.player2HitCount >= model.maxHitCount)
                        _gameStateStore.Dispatch(new CompleteGameAction(GamePlayer.Player2));
                })
                .Subscribe();

        private readonly IStore<GameStateModel> _gameStateStore;
    }
}
