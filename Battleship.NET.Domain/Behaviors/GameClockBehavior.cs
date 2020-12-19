using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;

using Redux;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Behaviors
{
    public class GameClockBehavior
        : IBehavior
    {
        public static readonly TimeSpan UpdateInterval
            = TimeSpan.FromMilliseconds(1000 / 60);

        public GameClockBehavior(
            IStore<GameStateModel> gameStateStore,
            ISystemClock systemClock)
        {
            _gameStateStore = gameStateStore;
            _systemClock = systemClock;
        }

        public IDisposable Start(IScheduler scheduler)
            => Observable.Timer(TimeSpan.Zero, UpdateInterval, scheduler)
                .Do(_ => _gameStateStore.Dispatch(new UpdateRuntimeAction(_systemClock.UtcNow.UtcDateTime)))
                .Subscribe();

        private readonly IStore<GameStateModel> _gameStateStore;
        private readonly ISystemClock _systemClock;
    }
}
