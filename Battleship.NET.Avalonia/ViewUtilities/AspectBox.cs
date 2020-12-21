using Avalonia;
using Avalonia.Controls;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public class AspectBox
        : Decorator
    {
        public double? AspectRatio
        {
            get => GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }
        public static readonly StyledProperty<double?> AspectRatioProperty
            = AvaloniaProperty.Register<AspectBox, double?>(nameof(AspectRatio));

        protected override Size ArrangeOverride(Size finalSize)
            => base.ArrangeOverride(ResolveToAspectRatio(finalSize));

        protected override Size MeasureOverride(Size availableSize)
            => base.MeasureOverride(ResolveToAspectRatio(availableSize));

        private Size ResolveToAspectRatio(Size size)
        {
            if (AspectRatio is not double targetAspectRatio)
                return size;

            var aspectRatio = size.Width / size.Height;

            return aspectRatio switch
            {
                _ when (aspectRatio > targetAspectRatio)    => new Size(size.Height * targetAspectRatio, size.Height),
                _ when (aspectRatio < targetAspectRatio)    => new Size(size.Width, size.Width / targetAspectRatio),
                _                                           => size
            };
        }
    }
}
