using System.Drawing;

using Battleship.NET.Domain.Models;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class MoveShipAction
        : IAction
    {
        public MoveShipAction(
            Rotation orientation,
            GamePlayer player,
            Point position,
            int shipIndex)
        {
            Orientation = orientation;
            Player = player;
            Position = position;
            ShipIndex = shipIndex;
        }

        public Rotation Orientation { get; }

        public GamePlayer Player { get; }

        public Point Position { get; }

        public int ShipIndex { get; }
    }
}
