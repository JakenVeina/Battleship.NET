using System;
using System.Reactive.Concurrency;

namespace Battleship.NET.Domain.Behaviors
{
    public interface IBehavior
    {
        public IDisposable Start(IScheduler scheduler);
    }
}
