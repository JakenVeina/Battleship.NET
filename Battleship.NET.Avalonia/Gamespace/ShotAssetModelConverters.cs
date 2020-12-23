using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Battleship.NET.WPF.Gamespace
{
    public static class ShotAssetModelConverters
    {
        public static readonly IMultiValueConverter LookupSpriteResource
            = new ShotAssetModelLookupResourceKeyValueConverter();

        private class ShotAssetModelLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is FrameworkElement frameworkElement)
                        && (values[1] is ShotAssetModel model)
                    ? frameworkElement.TryFindResource(
                        $"Sprite_Shot_{(model.IsHit ? "Hit" : "Miss")}")
                    : null;

            public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
