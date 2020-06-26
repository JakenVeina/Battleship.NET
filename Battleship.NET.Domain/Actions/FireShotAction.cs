using System.Drawing;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class FireShotAction
        : IAction
    {
        public FireShotAction(
            Point position)
        {
            Position = position;
        }

        public Point Position { get; }
    }
}
