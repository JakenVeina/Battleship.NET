using System;

namespace Battleship.NET.Domain.Actions
{
    public class UpdateRuntimeAction
    {
        public UpdateRuntimeAction(
            DateTime now)
        {
            Now = now;
        }

        public DateTime Now { get; }
    }
}
