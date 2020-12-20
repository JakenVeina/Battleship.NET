﻿using System;
using System.Drawing;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public static class OrientationConverters
    {
        public static readonly IValueConverter ToAngle
            = new NegateValueConverter();

        private class NegateValueConverter
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