using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardStateModel
    {
        public static GameBoardStateModel CreateIdle(
                GameBoardDefinitionModel definition,
                IEnumerable<ShipDefinitionModel> ships)
            => new GameBoardStateModel(
                definition,
                ImmutableHashSet<Point>.Empty,
                ImmutableHashSet<Point>.Empty,
                ships
                    .Select(ship => ShipStateModel.CreateIdle(ship))
                    .ToImmutableList());

        public GameBoardStateModel(
            GameBoardDefinitionModel definition,
            ImmutableHashSet<Point> hits,
            ImmutableHashSet<Point> misses,
            ImmutableList<ShipStateModel> ships)
        {
            Definition = definition;
            Hits = hits;
            Misses = misses;
            Ships = ships;
        }


        public GameBoardDefinitionModel Definition { get; }

        public ImmutableHashSet<Point> Hits { get; }

        public ImmutableHashSet<Point> Misses { get; }

        public ImmutableList<ShipStateModel> Ships { get; }


        public bool IsValid
        {
            get
            {
                var visitedPoints = new HashSet<Point>();

                var occupiedPointsSequence = Ships.SelectMany(ship => ship.Definition.Points
                    .Select(point => point.RotateOrigin(ship.Orientation).Translate(ship.Position)));

                foreach (var occupiedPoint in occupiedPointsSequence)
                {
                    if (visitedPoints.Contains(occupiedPoint) || !Definition.Points.Contains(occupiedPoint))
                        return false;
                    visitedPoints.Add(occupiedPoint);
                }

                return true;
            }
        }


        public bool CanReceiveShot(
                Point position)
            => !Hits.Contains(position) && !Misses.Contains(position);


        public GameBoardStateModel MoveShip(
                int shipIndex,
                Rotation orientation,
                Point position)
            => new GameBoardStateModel(
                Definition,
                Hits,
                Misses,
                Ships.SetItem(shipIndex, Ships[shipIndex].Move(orientation, position)));

        public GameBoardStateModel ReceiveShot(
                Point position)
            => Ships.Any(ship => ship.Definition.Points.Contains(position))
                    ? new GameBoardStateModel(
                        Definition,
                        Hits.Add(position),
                        Misses,
                        Ships)
                    : new GameBoardStateModel(
                        Definition,
                        Hits,
                        Misses.Add(position),
                        Ships);

        public GameBoardStateModel Reset()
            => new GameBoardStateModel(
                Definition,
                ImmutableHashSet<Point>.Empty,
                ImmutableHashSet<Point>.Empty,
                Ships);
    }
}
