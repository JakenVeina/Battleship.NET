using System.Windows;
using System.Windows.Controls;

namespace Battleship.NET.WPF.ViewUtilities
{
    public class AspectBox
        : Decorator
    {
        public double? AspectRatio
        {
            get => (double?)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }
        public static readonly DependencyProperty AspectRatioProperty
            = DependencyProperty.Register(
                nameof(AspectRatio),
                typeof(double?),
                typeof(AspectBox),
                new FrameworkPropertyMetadata(
                    defaultValue:   null,
                    flags:          FrameworkPropertyMetadataOptions.AffectsArrange
                                        | FrameworkPropertyMetadataOptions.AffectsMeasure
                                        | FrameworkPropertyMetadataOptions.AffectsRender));

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
