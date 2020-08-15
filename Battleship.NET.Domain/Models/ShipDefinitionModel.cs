using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class ShipDefinitionModel
    {
        public static ShipDefinitionModel Create(
                string name,
                IEnumerable<Point> segments)
            => new ShipDefinitionModel(
                name,
                segments.ToImmutableHashSet());

        public ShipDefinitionModel(
            string name,
            ImmutableHashSet<Point> segments)
        {
            Name = name;
            Segments = segments;
        }

        public string Name { get; }

        public ImmutableHashSet<Point> Segments { get; }
    }
}
