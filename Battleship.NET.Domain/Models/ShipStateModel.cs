using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipStateModel
    {
        public static ShipStateModel CreateIdle(
                ShipDefinitionModel definitionModel)
            => new ShipStateModel(
                definitionModel,
                default,
                default);

        public ShipStateModel(
            ShipDefinitionModel definition,
            Rotation orientation,
            Point position)
        {
            Definition = definition;
            Orientation = orientation;
            Position = position;
        }


        public ShipDefinitionModel Definition { get; }

        public Rotation Orientation { get; }

        public Point Position { get; }


        public ShipStateModel Move(
                Rotation orientation,
                Point position)
            => new ShipStateModel(
                Definition,
                orientation,
                position);
    }
}
