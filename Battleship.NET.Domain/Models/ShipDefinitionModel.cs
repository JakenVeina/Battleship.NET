using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipDefinitionModel
    {
        public static ShipDefinitionModel Create(
                string name,
                IEnumerable<Point> points)
            => new ShipDefinitionModel(
                name,
                points.ToImmutableHashSet());

        public ShipDefinitionModel(
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
