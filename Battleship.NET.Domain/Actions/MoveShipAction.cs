﻿using System.ComponentModel;
using System.Drawing;

using Battleship.NET.Domain.Models;

using Redux;

namespace Battleship.NET.Domain.Actions
{
    public class MoveShipAction
        : IAction
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
