using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Gamespace
{
    public static class ShotOutcomeConverters
    {
        public static readonly IMultiValueConverter LookupSpriteResource
            = new ShotAssetModelLookupResourceKeyValueConverter();

        private class ShotAssetModelLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is FrameworkElement frameworkElement)
                        && (values[1] is ShotOutcome outcome)
                    ? frameworkElement.TryFindResource(
                        $"Sprite_Shot_{outcome}")
                    : null;

            public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
