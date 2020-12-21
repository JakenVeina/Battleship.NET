using System;
using System.Globalization;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public static class Int32Converters
    {
        public static readonly IValueConverter ToUniformColumnDefinitions
            = new ToUniformColumnDefinitionsValueConverter();

        public static readonly IValueConverter ToUniformRowDefinitions
            = new ToUniformRowDefinitionsValueConverter();

        private class ToUniformColumnDefinitionsValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
                if (value is not int columnCount)
                    return null;

                var result = new ColumnDefinitions();
                foreach (var _ in Enumerable.Range(0, columnCount))
                    result.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
                return result;
            }

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }

        private class ToUniformRowDefinitionsValueConverter
            : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
                if (value is not int rowCount)
                    return null;

                var result = new RowDefinitions();
                foreach (var _ in Enumerable.Range(0, rowCount))
                    result.Add(new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });
                return result;
            }

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
