using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Battleship.NET.WPF.ViewUtilities
{
    public static class ConvertibleConverters
    {
        public static readonly IValueConverter ToStarGridLength
            = new ToGridLengthConverter(GridUnitType.Star);

        private class ToGridLengthConverter
            : IValueConverter
        {
            public ToGridLengthConverter(GridUnitType gridUnitType)
            {
                _gridUnitType = gridUnitType;
            }

            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
                => value switch
                {
                    IConvertible convertible    => new GridLength(convertible.ToDouble(null), _gridUnitType).ToNullable(),
                    _                           => null
                };

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();

            private readonly GridUnitType _gridUnitType;
        }
    }
}
