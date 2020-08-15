using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public static class StringConverters
    {
        public static readonly IMultiValueConverter Concat
            = new StringConcatValueConverter();

        private class StringConcatValueConverter
            : IMultiValueConverter
        {
            public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
                => string.Concat(values);
        }
    }
}
