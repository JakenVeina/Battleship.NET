using System;
using System.Globalization;
using System.Windows.Data;

namespace Battleship.NET.WPF.ViewUtilities
{
    public static class TimeSpanConverters
    {
        public static readonly IValueConverter Format
            = new FormatValueConverter();

        private class FormatValueConverter
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
