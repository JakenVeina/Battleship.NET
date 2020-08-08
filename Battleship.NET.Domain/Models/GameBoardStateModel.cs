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
                ImmutableHashSet<Point>.Empty,
                ImmutableHashSet<Point>.Empty,
                ships
                    .Select(ship => ShipStateModel.Idle)
                    .ToImmutableList());

        public GameBoardStateModel(
            ImmutableHashSet<Point> hits,
            ImmutableHashSet<Point> misses,
            ImmutableList<ShipStateModel> ships)
        {
            Hits = hits;
            Misses = misses;
            Ships = ships;
        }


        public ImmutableHashSet<Point> Hits { get; }

        public ImmutableHashSet<Point> Misses { get; }

        public ImmutableList<ShipStateModel> Ships { get; }


        public bool CanReceiveShot(
                Point position)
            => !Hits.Contains(position) && !Misses.Contains(position);

        public bool IsValid(
            GameBoardDefinitionModel definition,
            IEnumerable<ShipDefinitionModel> shipDefinitions)
        {
            var visitedPoints = new HashSet<Point>();

            var occupiedPointsSequence = Ships
                .Zip(shipDefinitions, (ship, definition) => (ship, definition))
                .SelectMany(x => x.ship.EnumeratePositions(x.definition));

            foreach (var occupiedPoint in occupiedPointsSequence)
            {
                if (visitedPoints.Contains(occupiedPoint) || !definition.Positions.Contains(occupiedPoint))
                    return false;
                visitedPoints.Add(occupiedPoint);
            }

            return true;
        }

        public bool IsValid(
            Point position,
            IEnumerable<ShipDefinitionModel> shipDefinitions)
        {
            var shipsAtPosition = Ships
                .Zip(shipDefinitions, (ship, definition) => (ship, definition))
                .SelectMany(x => x.ship.EnumeratePositions(x.definition))
                .Count(point => point == position);

            return (shipsAtPosition <= 1);
        }


        public GameBoardStateModel MoveShip(
                int shipIndex,
                Rotation orientation,
                Point position)
            => new GameBoardStateModel(
                Hits,
                Misses,
                Ships.SetItem(shipIndex, Ships[shipIndex].Move(orientation, position)));

        public GameBoardStateModel ReceiveShot(
                Point position,
                IEnumerable<ShipDefinitionModel> shipDefinitions)
            => Ships.Zip(shipDefinitions, (ship, definition) => (ship, definition))
                    .SelectMany(x => x.ship.EnumeratePositions(x.definition))
                    .Contains(position)
                ? new GameBoardStateModel(
                    Hits.Add(position),
                    Misses,
                    Ships)
                : new GameBoardStateModel(
                    Hits,
                    Misses.Add(position),
                    Ships);

        public GameBoardStateModel Reset()
            => new GameBoardStateModel(
                ImmutableHashSet<Point>.Empty,
                ImmutableHashSet<Point>.Empty,
                Ships);
    }
}
