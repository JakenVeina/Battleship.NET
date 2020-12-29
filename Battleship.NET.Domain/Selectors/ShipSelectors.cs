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
        public static readonly SelectorSet<GameStateModel, ImmutableArray<ShipSegmentPlacementModel>, GamePlayer> AllSegmentPlacements
            = SelectorSet.Create<GameStateModel, ImmutableArray<ShipSegmentPlacementModel>, GamePlayer>(
                player => Selector.Create<GameStateModel, IReadOnlyList<ImmutableArray<ShipSegmentPlacementModel>>, ImmutableArray<ShipSegmentPlacementModel>>(
                    argSelector:    gameState => Enumerable.Range(0, gameState.Definition.Ships.Length)
                        .Select(index => SegmentPlacements![(player, index)].Invoke(gameState)) // Null-check overridden because this can only be null of SelectorSet.Create() and Selector.Create() both invoke their delegates immediately, which neither do.
                        .ToImmutableArray(),
                    resultSelector: segmentPlacementSet => segmentPlacementSet
                        .SelectMany(segmentPlacements => segmentPlacements)
                        .ToImmutableArray(),
                    argComparer:    SequenceEqualityComparer<ImmutableArray<ShipSegmentPlacementModel>>.Default));

        public static readonly SelectorSet<GameStateModel, bool, (GamePlayer player, int index)> IsSunk
            = SelectorSet.Create<GameStateModel, bool, (GamePlayer player, int index)>(
                @params => Selector.Create<GameStateModel, ImmutableHashSet<Point>, ImmutableArray<ShipSegmentPlacementModel>, bool>(
                    arg1Selector:   (@params.player == GamePlayer.Player1)
                        ? gameState => gameState.Player1.GameBoard.Hits
                        : gameState => gameState.Player2.GameBoard.Hits,
                    arg2Selector:   SegmentPlacements![@params],  // Null-check overridden because this can only be null of SelectorSet.Create() and Selector.Create() both invoke their delegates immediately, which neither do.
                    resultSelector: (hits, segmentPlacements) => segmentPlacements
                        .All(segmentPlacement => hits.Contains(segmentPlacement.Position))));

        public static readonly SelectorSet<GameStateModel, bool, (GamePlayer player, int index)> IsValid
            = SelectorSet.Create<GameStateModel, bool, (GamePlayer player, int index)>(
                @params => Selector.Create(
                    argSelector:    AllSegmentPlacements[@params.player],
                    resultSelector: segmentPlacements => !Enumerable.Intersect(
                            segmentPlacements
                                .Where(segmentPlacement => segmentPlacement.ShipIndex == @params.index)
                                .Select(segmentPlacement => segmentPlacement.Position),
                            segmentPlacements
                                .Where(segmentPlacement => segmentPlacement.ShipIndex != @params.index)
                                .Select(segmentPlacement => segmentPlacement.Position))
                        .Any()));

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

        public static readonly SelectorSet<GameStateModel, ImmutableArray<ShipSegmentPlacementModel>, (GamePlayer player, int index)> SegmentPlacements
            = SelectorSet.Create<GameStateModel, ImmutableArray<ShipSegmentPlacementModel>, (GamePlayer player, int index)>(
                @params => Selector.Create<GameStateModel, IReadOnlyList<ShipSegmentPlacementModel>, ImmutableArray<ShipSegmentPlacementModel>>(
                    argSelector:    gameState => gameState.Definition.Ships[@params.index].Segments
                        .Select(segment => SegmentPlacement[(@params.player, @params.index, segment)].Invoke(gameState))
                        .ToImmutableArray(),
                    resultSelector: segmentPlacements => (ImmutableArray<ShipSegmentPlacementModel>)segmentPlacements,
                    argComparer:    SequenceEqualityComparer<ShipSegmentPlacementModel>.Default));
    }
}
