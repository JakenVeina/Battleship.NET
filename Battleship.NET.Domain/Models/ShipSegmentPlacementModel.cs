using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipSegmentPlacementModel
    {
        public ShipSegmentPlacementModel(
            Orientation orientation,
            Point position,
            Point segment,
            int shipIndex)
        {
            Orientation = orientation;
            Position    = position;
            Segment     = segment;
            ShipIndex   = shipIndex;
        }

        public Orientation Orientation { get; }

        public Point Position { get; }

        public Point Segment { get; }

        public int ShipIndex { get; }
    }
}
