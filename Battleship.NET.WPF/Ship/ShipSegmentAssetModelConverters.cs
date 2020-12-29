using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Battleship.NET.WPF.Ship
{
    public static class ShipSegmentAssetModelConverters
    {
        public static readonly IMultiValueConverter LookupSpriteResource
            = new ShipAssetModelLookupResourceKeyValueConverter();

        private class ShipAssetModelLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is FrameworkElement frameworkElement)
                        && (values[1] is ShipSegmentAssetModel model)
                    ? frameworkElement.TryFindResource(
                        $"Sprite_Ship_Standard_{model.ShipName.Replace(" ", "")}_{model.Segment.X}_{model.Segment.Y}")
                    : null;

            public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
