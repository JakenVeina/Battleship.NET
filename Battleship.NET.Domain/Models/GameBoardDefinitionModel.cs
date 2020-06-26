using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class GameBoardDefinitionModel
    {
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
