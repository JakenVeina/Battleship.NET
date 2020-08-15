using System.Drawing;

namespace Battleship.NET.Avalonia.Ship
{
    public class ShipSegmentAssetModel
    {
        public ShipSegmentAssetModel(
            Point segment,
            string shipName)
        {
            Segment = segment;
            ShipName = shipName;
        }

        public Point Segment { get; }

        public string ShipName { get; }
    }
}
