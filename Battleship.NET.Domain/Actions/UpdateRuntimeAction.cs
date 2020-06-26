using System;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class UpdateRuntimeAction
        : IAction
    {
        public UpdateRuntimeAction(
            DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; }
    }
}
