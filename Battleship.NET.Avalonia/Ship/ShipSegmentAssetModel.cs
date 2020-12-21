using System.Drawing;

namespace Battleship.NET.Avalonia.Ship
{
    public class ShipSegmentAssetModel
    {
        public ShipSegmentAssetModel(
            int index,
            string name,
            Point segment)
        {
            Index = index;
            Name = name;
            Segment = segment;
        }

        public int Index { get; }
        
        public string Name { get; }

        public Point Segment { get; }
    }
}
