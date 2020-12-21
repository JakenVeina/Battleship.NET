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
            = new ShipAssetModelLookupResourceKeyValueConverter();

        private class ShipAssetModelLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is IResourceHost resourceHost)
                        && (values[1] is ShipSegmentAssetModel model)
                        && resourceHost.TryFindResource(
                            $"Sprite_Ship_Standard_{model.Name.Replace(" ", "")}_{model.Segment.X}_{model.Segment.Y}",
                            out var resource)
                    ? resource
                    : null;
        }
    }
}
