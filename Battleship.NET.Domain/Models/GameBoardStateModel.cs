﻿using System.Collections.Generic;
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

            var occupiedPositionsSequence = Ships
                .Zip(shipDefinitions, (ship, definition) => (ship, definition))
                .SelectMany(x => x.ship.EnumerateSegmentPositions(x.definition));

            foreach (var occupiedPosition in occupiedPositionsSequence)
            {
                if (visitedPoints.Contains(occupiedPosition) || !definition.Positions.Contains(occupiedPosition))
                    return false;
                visitedPoints.Add(occupiedPosition);
            }

            return true;
        }

        public bool IsValid(
            Point position,
            IEnumerable<ShipDefinitionModel> shipDefinitions)
        {
            var shipsAtPosition = Ships
                .Zip(shipDefinitions, (ship, definition) => (ship, definition))
                .SelectMany(x => x.ship.EnumerateSegmentPositions(x.definition))
                .Count(segmentPosition => segmentPosition == position);

            return (shipsAtPosition <= 1);
        }


        public GameBoardStateModel MoveShip(
                int shipIndex,
                Point shipSegment,
                Point targetPosition)
            => new GameBoardStateModel(
                Hits,
                Misses,
                Ships.SetItem(shipIndex, Ships[shipIndex].Move(shipSegment, targetPosition)));

        public GameBoardStateModel ReceiveShot(
                Point position,
                IEnumerable<ShipDefinitionModel> shipDefinitions)
            => Ships.Zip(shipDefinitions, (ship, definition) => (ship, definition))
                    .SelectMany(x => x.ship.EnumerateSegmentPositions(x.definition))
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

        public GameBoardStateModel RotateShip(
                int shipIndex,
                Point shipSegment,
                Rotation targetOrientation)
            => new GameBoardStateModel(
                Hits,
                Misses,
                Ships.SetItem(shipIndex, Ships[shipIndex].Rotate(shipSegment, targetOrientation)));
    }
}