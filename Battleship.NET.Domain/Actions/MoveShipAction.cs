using System.Drawing;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class MoveShipAction
    {
        public MoveShipAction(
            GamePlayer player,
            int shipIndex,
            Point shipSegment,
            Point targetPosition)
        {
            Player = player;
            ShipIndex = shipIndex;
            ShipSegment = shipSegment;
            TargetPosition = targetPosition;
        }

        public GamePlayer Player { get; }

        public int ShipIndex { get; }

        public Point ShipSegment { get; }

        public Point TargetPosition { get; }
    }
}
