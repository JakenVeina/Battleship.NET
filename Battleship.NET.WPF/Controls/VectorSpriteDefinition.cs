using System.Windows;
using System.Windows.Media;

namespace Battleship.NET.WPF.Controls
{
    public class VectorSpriteDefinition
        : DependencyObject
    {
        public double CanvasHeight
        {
            get => (double)GetValue(CanvasHeightProperty);
            set => SetValue(CanvasHeightProperty, value);
        }
        private static readonly DependencyProperty CanvasHeightProperty
            = DependencyProperty.Register(
                nameof(CanvasHeight),
                typeof(double),
                typeof(VectorSpriteDefinition));

        public double CanvasWidth
        {
            get => (double)GetValue(CanvasWidthProperty);
            set => SetValue(CanvasWidthProperty, value);
        }
        private static readonly DependencyProperty CanvasWidthProperty
            = DependencyProperty.Register(
                nameof(CanvasWidth),
                typeof(double),
                typeof(VectorSpriteDefinition));

        public Geometry? Geometry
        {
            get => (Geometry?)GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }
        private static readonly DependencyProperty GeometryProperty
            = DependencyProperty.Register(
                nameof(Geometry),
                typeof(Geometry),
                typeof(VectorSpriteDefinition));
    }
}
