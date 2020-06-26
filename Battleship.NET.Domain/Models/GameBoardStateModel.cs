using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardStateModel
    {
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
    }
}
