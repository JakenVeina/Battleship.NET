using System;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class UpdateRuntimeAction
        : IAction
    {
        public UpdateRuntimeAction(
            DateTime now)
        {
            Now = now;
        }

        public DateTime Now { get; }
    }
}
