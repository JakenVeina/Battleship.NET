using System.Drawing;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class RotateShipAction
    {
        public RotateShipAction(
            GamePlayer player,
            int shipIndex,
            Point shipSegment,
            Orientation targetOrientation)
        {
            Player = player;
            ShipIndex = shipIndex;
            ShipSegment = shipSegment;
            TargetOrientation = targetOrientation;
        }

        public GamePlayer Player { get; }

        public int ShipIndex { get; }

        public Point ShipSegment { get; }

        public Orientation TargetOrientation { get; }
    }
}
