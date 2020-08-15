using Avalonia;
using Avalonia.Media;

namespace Battleship.NET.Avalonia.Controls
{
    public class VectorSpriteDefinition
        : AvaloniaObject
    {
        public double CanvasHeight
        {
            get => GetValue(CanvasHeightProperty);
            set => SetValue(CanvasHeightProperty, value);
        }
        private static readonly StyledProperty<double> CanvasHeightProperty
            = AvaloniaProperty.Register<VectorSpriteDefinition, double>(nameof(CanvasHeight));

        public double CanvasWidth
        {
            get => GetValue(CanvasWidthProperty);
            set => SetValue(CanvasWidthProperty, value);
        }
        private static readonly StyledProperty<double> CanvasWidthProperty
            = AvaloniaProperty.Register<VectorSpriteDefinition, double>(nameof(CanvasWidth));

        public Geometry Geometry
        {
            get => GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }
        private static readonly StyledProperty<Geometry> GeometryProperty
            = AvaloniaProperty.Register<VectorSpriteDefinition, Geometry>(nameof(Geometry));
    }
}
