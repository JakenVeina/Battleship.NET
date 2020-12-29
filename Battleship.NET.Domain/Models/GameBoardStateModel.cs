using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardStateModel
    {
        public static GameBoardStateModel CreateIdle(
                IEnumerable<ShipDefinitionModel> ships)
            => new GameBoardStateModel(
                hits:   ImmutableHashSet<Point>.Empty,
                misses: ImmutableHashSet<Point>.Empty,
                ships:  ships
                    .Select(ship => ShipStateModel.Idle)
                    .ToImmutableList());

        private GameBoardStateModel(
            ImmutableHashSet<Point> hits,
            ImmutableHashSet<Point> misses,
            ImmutableList<ShipStateModel> ships)
        {
            Hits   = hits;
            Misses = misses;
            Ships  = ships;
        }

        private GameBoardStateModel(GameBoardStateModel original)
        {
            Hits   = original.Hits;
            Misses = original.Misses;
            Ships  = original.Ships;
        }


        public ImmutableHashSet<Point> Hits { get; private init; }

        public ImmutableHashSet<Point> Misses { get; private init; }

        public ImmutableList<ShipStateModel> Ships { get; private init; }


        public GameBoardStateModel ClearShots()
            => new(this)
            {
                Hits    = Hits.Clear(),
                Misses  = Misses.Clear()
            };

        public GameBoardStateModel MoveShip(
                int shipIndex,
                Point shipSegment,
                Point targetPosition)
            => new(this)
            {
                Ships = Ships.SetItem(shipIndex, Ships[shipIndex].Move(shipSegment, targetPosition))
            };

        public GameBoardStateModel PlaceShips(
                ImmutableList<ShipStateModel> ships)
            => new(this)
            {
                Ships = ships
            };

        public GameBoardStateModel ReceiveShot(
            Point position,
            ShotOutcome outcome)
        {
            var (hits, misses) = (outcome == ShotOutcome.Hit)
                ? (Hits.Add(position), Misses)
                : (Hits, Misses.Add(position));

            return new(this)
            {
                Hits    = hits,
                Misses  = misses
            };
        }

        public GameBoardStateModel RotateShip(
                int shipIndex,
                Point shipSegment,
                Orientation targetOrientation)
            => new(this)
            {
                Ships = Ships.SetItem(shipIndex, Ships[shipIndex].Rotate(shipSegment, targetOrientation))
            };
    }
}
