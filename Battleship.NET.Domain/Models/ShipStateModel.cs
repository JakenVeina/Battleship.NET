using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipStateModel
    {
        public ShipStateModel(
            ShipDefinitionModel definition,
            Orientation orientation,
            Point position)
        {
            Definition = definition;
            Orientation = orientation;
            Position = position;
        }

        public ShipDefinitionModel Definition { get; }

        public Orientation Orientation { get; }

        public Point Position { get; }
    }
}
