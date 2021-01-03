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
                // TODO: Implement this
                player => throw new NotImplementedException());

        public static readonly SelectorSet<GameStateModel, ShipSegmentPlacementModel, (GamePlayer player, int index, Point segment)> SegmentPlacement
            = SelectorSet.Create<GameStateModel, ShipSegmentPlacementModel, (GamePlayer player, int index, Point segment)>(
                // TODO: Implement this
                @params => throw new NotImplementedException());

        public static readonly SelectorSet<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, (GamePlayer player, int index)> SegmentPlacements
            = SelectorSet.Create<GameStateModel, ReadOnlyValueList<ShipSegmentPlacementModel>, (GamePlayer player, int index)>(
                // TODO: Implement this
                @params => throw new NotImplementedException());
    }
}
