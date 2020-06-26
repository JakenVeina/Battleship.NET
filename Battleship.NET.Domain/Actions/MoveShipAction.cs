using System.Drawing;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class MoveShipAction
        : IAction
    {
        public MoveShipAction(
            Orientation orientation,
            Point position,
            int shipIndex)
        {
            Orientation = orientation;
            Position = position;
            ShipIndex = shipIndex;
        }

        public Orientation Orientation { get; }

        public Point Position { get; }

        public int ShipIndex { get; }
    }
}
