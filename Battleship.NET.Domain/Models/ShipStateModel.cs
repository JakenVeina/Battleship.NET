using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Battleship.NET.Domain.Models
{
    public class ShipStateModel
    {
        public static readonly ShipStateModel Idle
            = new ShipStateModel(
                default,
                default);

        public ShipStateModel(
            Rotation orientation,
            Point position)
        {
            Orientation = orientation;
            Position = position;
        }


        public Rotation Orientation { get; }

        public Point Position { get; }


        public IEnumerable<Point> EnumeratePositions(
                ShipDefinitionModel definition)
            => definition.Points
                .Select(point => point.RotateOrigin(Orientation).Translate(Position));

        public ShipStateModel Move(
                Rotation orientation,
                Point position)
            => new ShipStateModel(
                orientation,
                position);
    }
}
