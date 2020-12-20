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
            Orientation orientation,
            Point position)
        {
            Orientation = orientation;
            Position = position;
        }


        public Orientation Orientation { get; }

        public Point Position { get; }


        public IEnumerable<Point> EnumerateSegmentPositions(
                ShipDefinitionModel definition)
            => definition.Segments
                .Select(segment => segment.RotateOrigin(Orientation).Translate(Position));

        public ShipStateModel Move(
                Point shipSegment,
                Point targetPosition)
            => new ShipStateModel(
                Orientation,
                Position
                    .Translate(shipSegment
                        .RotateOrigin(Orientation)
                        .Translate(Position)
                        .Negate())
                    .Translate(targetPosition));

        public ShipStateModel Rotate(
                Point shipSegment,
                Orientation targetOrientation)
            => new ShipStateModel(
                targetOrientation,
                Position
                    .Translate(shipSegment
                        .RotateOrigin(targetOrientation)
                        .Translate(Position)
                        .Negate())
                    .Translate(shipSegment
                        .RotateOrigin(Orientation)
                        .Translate(Position)));
    }
}
