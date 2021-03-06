﻿using System.Drawing;

namespace Battleship.NET.WPF.Ship
{
    public class ShipSegmentAssetModel
    {
        public ShipSegmentAssetModel(
            Orientation orientation,
            Point segment,
            int shipIndex,
            string shipName)
        {
            Orientation = orientation;
            Segment     = segment;
            ShipIndex   = shipIndex;
            ShipName    = shipName;
        }

        public Orientation Orientation { get; }

        public Point Segment { get; }

        public int ShipIndex { get; }

        public string ShipName { get; }
    }
}
