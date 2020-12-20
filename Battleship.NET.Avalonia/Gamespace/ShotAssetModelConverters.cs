using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.Gamespace
{
    public static class ShotAssetModelConverters
    {
        public static readonly IMultiValueConverter LookupSpriteResource
            = new ShotAssetModelLookupResourceKeyValueConverter();

        private class ShotAssetModelLookupResourceKeyValueConverter
            : IMultiValueConverter
        {
            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
                => (values[0] is IResourceHost resourceHost)
                        && (values[1] is ShotAssetModel model)
                        && resourceHost.TryFindResource(
                            $"Sprite_Shot_{(model.IsHit ? "Hit" : "Miss")}",
                            out var resource)
                    ? resource
                    : null;
        }
    }
}
