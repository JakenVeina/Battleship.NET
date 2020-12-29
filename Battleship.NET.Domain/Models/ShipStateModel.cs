using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipStateModel
    {
        public static readonly ShipStateModel Idle
            = new ShipStateModel(
                orientation:    default,
                position:       default);

        public static ShipStateModel Place(
                Orientation orientation,
                Point position)
            => new ShipStateModel(
                orientation:    orientation,
                position:       position);

        private ShipStateModel(
            Orientation orientation,
            Point position)
        {
            Orientation = orientation;
            Position    = position;
        }

        private ShipStateModel(ShipStateModel original)
        {
            Orientation = original.Orientation;
            Position    = original.Position;
        }


        public Orientation Orientation { get; private init; }

        public Point Position { get; private init; }


        public ShipStateModel Move(
                Point segment,
                Point targetPosition)
            => new(this)
            {
                Position = Position
                    .Translate(segment
                        .RotateOrigin(Orientation)
                        .Translate(Position)
                        .Negate())
                    .Translate(targetPosition)
            };

        public ShipStateModel Rotate(
                Point shipSegment,
                Orientation targetOrientation)
            => new(this)
            {
                Orientation = targetOrientation,
                Position    = Position
                    .Translate(shipSegment
                        .RotateOrigin(targetOrientation)
                        .Translate(Position)
                        .Negate())
                    .Translate(shipSegment
                        .RotateOrigin(Orientation)
                        .Translate(Position))
            };
    }
}
