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
                name,
                positions.ToImmutableHashSet());

        public GameBoardDefinitionModel(
            string name,
            ImmutableHashSet<Point> positions)
        {
            Name = name;
            Positions = positions;
        }

        public string Name { get; }

        public ImmutableHashSet<Point> Positions { get; }
    }
}
