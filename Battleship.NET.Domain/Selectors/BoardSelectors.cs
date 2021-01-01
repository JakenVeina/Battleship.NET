using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Selectors
{
    public static class BoardSelectors
    {
        public static readonly SelectorSet<GameStateModel, bool, (GamePlayer player, Point position)> CanReceiveShot
            = SelectorSet.Create<GameStateModel, bool, (GamePlayer player, Point position)>(@params =>
            {
                Func<GameStateModel, ImmutableHashSet<Point>> hitsSelector, missesSelector;
                if (@params.player == GamePlayer.Player1)
                {
                    hitsSelector    = gameState => gameState.Player1.GameBoard.Hits;
                    missesSelector  = gameState => gameState.Player1.GameBoard.Misses;
                }
                else
                {
                    hitsSelector    = gameState => gameState.Player2.GameBoard.Hits;
                    missesSelector  = gameState => gameState.Player2.GameBoard.Misses;
                }

                return Selector.Create(
                    hitsSelector,
                    missesSelector,
                    (hits, misses) => !hits.Contains(@params.position)
                        && !misses.Contains(@params.position));
            });

        public static readonly Func<GameStateModel, ImmutableArray<string>> ColumnHeadings
            = Selector.Create<GameStateModel, GameDefinitionModel, ImmutableArray<string>>(
                    argSelector:    gameState => gameState.Definition,
                    resultSelector: definition => definition.GameBoard.Positions
                        .Select(position => position.X)
                        .Distinct()
                        .OrderBy(x => x)
                        .Select(x => x.ToString())
                        .ToImmutableArray());

        public static readonly SelectorSet<GameStateModel, bool, GamePlayer> IsValid
            = SelectorSet.Create<GameStateModel, bool, GamePlayer>(
                player => Selector.Create(
                    arg1Selector:   gameState => gameState.Definition,
                    arg2Selector:   ShipSelectors.AllSegmentPlacements[player],
                    resultSelector: (definition, segmentPlacements) =>
                    {
                        var visitedPositions = new HashSet<Point>();

                        foreach (var position in segmentPlacements.Select(segmentPlacement => segmentPlacement.Position))
                        {
                            if (visitedPositions.Contains(position) || !definition.GameBoard.Positions.Contains(position))
                                return false;
                            visitedPositions.Add(position);
                        }

                        return true;
                    }));

        public static readonly Func<GameStateModel, ImmutableArray<string>> RowHeadings
            = Selector.Create<GameStateModel, GameDefinitionModel, ImmutableArray<string>>(
                    argSelector:    gameState => gameState.Definition,
                    resultSelector: definition => definition.GameBoard.Positions
                        .Select(position => position.Y)
                        .Distinct()
                        .OrderBy(y => y)
                        .Select(y => Convert.ToChar(y + 'A').ToString())
                        .ToImmutableArray());

        public static readonly SelectorSet<GameStateModel, ShipSegmentPlacementModel?, (GamePlayer player, Point position)> ShipSegmentPlacement
            = SelectorSet.Create<GameStateModel, ShipSegmentPlacementModel?, (GamePlayer player, Point position)>(
                @params => Selector.Create<GameStateModel, ImmutableArray<ShipSegmentPlacementModel>, ShipSegmentPlacementModel?>(
                    argSelector:    ShipSelectors.AllSegmentPlacements[@params.player],
                    resultSelector: segmentPlacements => segmentPlacements
                        .Where(segmentPlacement => segmentPlacement.Position == @params.position)
                        .FirstOrDefault()));

        public static readonly SelectorSet<GameStateModel, ShotOutcome?, (GamePlayer player, Point position)> ShotOutcome
            = SelectorSet.Create<GameStateModel, ShotOutcome?, (GamePlayer player, Point position)>(
                @params => Selector.Create<GameStateModel, ImmutableHashSet<Point>, ImmutableHashSet<Point>, ShotOutcome?>(
                    arg1Selector:   (@params.player == GamePlayer.Player1)
                        ? gameState => gameState.Player1.GameBoard.Hits
                        : gameState => gameState.Player2.GameBoard.Hits,
                    arg2Selector:   (@params.player == GamePlayer.Player1)
                        ? gameState => gameState.Player1.GameBoard.Misses
                        : gameState => gameState.Player2.GameBoard.Misses,
                    resultSelector: (hits, misses) => @params.position switch
                    {
                        _ when hits.Contains(@params.position)      => Models.ShotOutcome.Hit,
                        _ when misses.Contains(@params.position)    => Models.ShotOutcome.Miss,
                        _                                           => null
                    }));

        public static readonly Func<GameStateModel, Size> Size
            = Selector.Create<GameStateModel, GameDefinitionModel, Size>(
                gameState => gameState.Definition,
                definition => new Size(
                    width:  definition.GameBoard.Positions.Max(position => position.X) + 1,
                    height: definition.GameBoard.Positions.Max(position => position.Y) + 1));
    }
}
