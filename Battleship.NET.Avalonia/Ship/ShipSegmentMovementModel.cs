using System.Drawing;

namespace Battleship.NET.Avalonia.Ship
{
    public class ShipSegmentMovementModel
    {
        public ShipSegmentMovementModel(
            Point segment,
            int shipIndex)
        {
            Segment = segment;
            ShipIndex = shipIndex;
        }

        public Point Segment { get; }

        public int ShipIndex { get; }
    }
}
