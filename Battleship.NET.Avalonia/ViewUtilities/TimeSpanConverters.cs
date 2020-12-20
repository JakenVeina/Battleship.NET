using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public static class TimeSpanConverters
    {
        public static readonly IValueConverter Format
            = new NegateValueConverter();

        private class NegateValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
                => value switch
                {
                    TimeSpan timeSpan   => timeSpan.ToString("hh\\:mm\\:ss"),
                    _                   => null
                };

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
