using System;
using System.Reactive.Concurrency;

namespace Battleship.NET.Domain.Behaviors
{
    public class GameClockBehavior
        : IBehavior
    {
        public static readonly TimeSpan UpdateInterval
            = TimeSpan.FromMilliseconds(1000 / 60);

        // TODO: Implement this
        public IDisposable Start(IScheduler scheduler)
            => throw new NotImplementedException();
    }
}
