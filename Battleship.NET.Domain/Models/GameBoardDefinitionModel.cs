using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardDefinitionModel
    {
        public static GameBoardDefinitionModel Create(
                string name,
                IEnumerable<Point> points)
            => new GameBoardDefinitionModel(
                name,
                points.ToImmutableHashSet());

        public GameBoardDefinitionModel(
            string name,
            ImmutableHashSet<Point> points)
        {
            Name = name;
            Points = points;
        }

        public string Name { get; }

        public ImmutableHashSet<Point> Points { get; }
    }
}
