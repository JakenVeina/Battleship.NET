using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Battleship.NET.WPF.ViewUtilities
{
    public static class OrientationConverters
    {
        public static readonly IValueConverter ToAngle
            = new ToAngleValueConverter();

        private class ToAngleValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
                => value switch
                {
                    Orientation.Rotate0     => 0,
                    Orientation.Rotate90    => 270,
                    Orientation.Rotate180   => 180,
                    Orientation.Rotate270   => 90,
                    _                       => null
                };

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
