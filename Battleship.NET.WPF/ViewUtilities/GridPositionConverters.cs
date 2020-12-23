using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Battleship.NET.WPF.ViewUtilities
{
    public static class GridPositionConverters
    {
        public static readonly IValueConverter IsEven
            = new IsEvenValueConverter();

        private class IsEvenValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
                => value switch
                {
                    Point point => ((point.X + point.Y) % 2) == 0,
                    _           => null
                };

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
