using System.Drawing;

namespace Battleship.NET.Domain.Actions
{
    public class FireShotAction
    {
        public FireShotAction(
            Point position)
        {
            Position = position;
        }

        public Point Position { get; }
    }
}
