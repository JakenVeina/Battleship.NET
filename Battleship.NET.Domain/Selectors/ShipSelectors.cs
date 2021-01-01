using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Selectors
{
    public static class ShipSelectors
    {
        public static readonly SelectorSet<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, GamePlayer> AllSegmentPlacements
            = SelectorSet.Create<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, GamePlayer>(
                player => Selector.Create<GameStateModel, ReadOnlyValueList<ReadOnlyValueList<ShipSegmentPlacementModel>>, ReadOnlyValueList<ShipSegmentPlacementModel>>(
                    argSelector:    gameState => Enumerable.Range(0, gameState.Definition.Ships.Length)
                        .Select(index => SegmentPlacements![(player, index)].Invoke(gameState)) // Null-check overridden because this can only be null of SelectorSet.Create() and Selector.Create() both invoke their delegates immediately, which neither do.
                        .ToReadOnlyValueList(),
                    resultSelector: segmentPlacementSet => segmentPlacementSet
                        .SelectMany(segmentPlacements => segmentPlacements)
                        .ToReadOnlyValueList()));

        public static readonly SelectorSet<GameStateModel, bool, (GamePlayer player, int index)> IsSunk
            = SelectorSet.Create<GameStateModel, bool, (GamePlayer player, int index)>(
                @params => Selector.Create<GameStateModel, ImmutableHashSet<Point>, ReadOnlyValueList<ShipSegmentPlacementModel>, bool>(
                    arg1Selector:   (@params.player == GamePlayer.Player1)
                        ? gameState => gameState.Player1.GameBoard.Hits
                        : gameState => gameState.Player2.GameBoard.Hits,
                    arg2Selector:   SegmentPlacements![@params],  // Null-check overridden because this can only be null of SelectorSet.Create() and Selector.Create() both invoke their delegates immediately, which neither do.
                    resultSelector: (hits, segmentPlacements) => segmentPlacements
                        .All(segmentPlacement => hits.Contains(segmentPlacement.Position))));

        public static readonly SelectorSet<GameStateModel, bool, (GamePlayer player, int index)> IsValid
            = SelectorSet.Create<GameStateModel, bool, (GamePlayer player, int index)>(
                @params => Selector.Create(
                    arg1Selector:   gameState => gameState.Definition.GameBoard.Positions,
                    arg2Selector:   AllSegmentPlacements[@params.player],
                    resultSelector: (boardPositions, segmentPlacements) =>
                    {
                        var thisShipPositions = new HashSet<Point>();
                        var otherShipPositions = new HashSet<Point>();

                        foreach(var segmentPlacement in segmentPlacements)
                            if (segmentPlacement.ShipIndex == @params.index)
                                thisShipPositions.Add(segmentPlacement.Position);
                            else
                                otherShipPositions.Add(segmentPlacement.Position);

                        return thisShipPositions.All(position => boardPositions.Contains(position))
                            && !thisShipPositions.Intersect(otherShipPositions).Any();
                    }));

        public static readonly SelectorSet<GameStateModel, ShipSegmentPlacementModel, (GamePlayer player, int index, Point segment)> SegmentPlacement
            = SelectorSet.Create<GameStateModel, ShipSegmentPlacementModel, (GamePlayer player, int index, Point segment)>(
                @params => Selector.Create<GameStateModel, ShipStateModel, ShipSegmentPlacementModel>(
                    argSelector:    (@params.player == GamePlayer.Player1)
                        ? gameState => gameState.Player1.GameBoard.Ships[@params.index]
                        : gameState => gameState.Player2.GameBoard.Ships[@params.index],
                    resultSelector: state => new ShipSegmentPlacementModel(
                        orientation:    state.Orientation,
                        position:       @params.segment
                            .RotateOrigin(state.Orientation)
                            .Translate(state.Position),
                        segment:        @params.segment,
                        shipIndex:      @params.index)));

        public static readonly SelectorSet<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, (GamePlayer player, int index)> SegmentPlacements
            = SelectorSet.Create<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, (GamePlayer player, int index)>(
                @params => Selector.Create<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>>(
                    resultSelector: gameState => gameState.Definition.Ships[@params.index].Segments
                        .Select(segment => SegmentPlacement[(@params.player, @params.index, segment)].Invoke(gameState))
                        .ToReadOnlyValueList()));
    }
}
