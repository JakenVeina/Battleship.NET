using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.Ship
{
    public static class ShipSegmentAssetModelConverters
    {
        public static readonly IMultiValueConverter LookupSpriteResource
            = new ShipAssetDataLookupResourceKeyValueConverter();

        private class ShipAssetDataLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is IResourceHost resourceHost)
                        && (values[1] is ShipSegmentAssetModel data)
                        && resourceHost.TryFindResource(
                            $"Sprite_Ship_Standard_{data.ShipName.Replace(" ", "")}_{data.Segment.X}_{data.Segment.Y}",
                            out var resource)
                    ? resource
                    : null;
        }
    }
}
