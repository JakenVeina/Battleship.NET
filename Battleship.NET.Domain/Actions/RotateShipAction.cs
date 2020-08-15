using System;
using System.Drawing;

using Battleship.NET.Domain.Models;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class RotateShipAction
        : IAction
    {
        public RotateShipAction(
            GamePlayer player,
            int shipIndex,
            Point shipSegment,
            Rotation targetOrientation)
        {
            Player = player;
            ShipIndex = shipIndex;
            ShipSegment = shipSegment;
            TargetOrientation = targetOrientation;
        }

        public GamePlayer Player { get; }

        public int ShipIndex { get; }

        public Point ShipSegment { get; }

        public Rotation TargetOrientation { get; }
    }
}
