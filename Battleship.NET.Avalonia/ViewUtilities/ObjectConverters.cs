using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public static class ObjectConverters
    {
        new public static readonly IValueConverter ToString
            = new ToStringValueConverter();

        private class ToStringValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
                => value?.ToString();

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
