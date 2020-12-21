using System.Drawing;

namespace Battleship.NET.Avalonia.Ship
{
    public class ShipSegmentAssetModel
    {
        public ShipSegmentAssetModel(
            int index,
            string name,
            Orientation orientation,
            Point segment)
        {
            Index = index;
            Name = name;
            Orientation = orientation;
            Segment = segment;
        }

        public int Index { get; }
        
        public string Name { get; }

        public Orientation Orientation { get; }

        public Point Segment { get; }
    }
}
