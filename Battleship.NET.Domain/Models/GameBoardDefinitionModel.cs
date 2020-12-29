using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardDefinitionModel
    {
        public static GameBoardDefinitionModel Create(
                string name,
                IEnumerable<Point> positions)
            => new GameBoardDefinitionModel(
                name:       name,
                positions:  positions.ToImmutableHashSet());

        private GameBoardDefinitionModel(
            string name,
            ImmutableHashSet<Point> positions)
        {
            Name        = name;
            Positions   = positions;
        }

        public string Name { get; private init; }

        public ImmutableHashSet<Point> Positions { get; private init; }
    }
}
