using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

namespace Battleship.NET.WPF.ViewUtilities
{
    public class UniformGridBehavior
        : Behavior<Grid>
    {
        public int ColumnCount
        {
            get => (int)GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }
        public static readonly DependencyProperty ColumnCountProperty
            = DependencyProperty.Register(
                nameof(ColumnCount),
                typeof(int),
                typeof(UniformGridBehavior),
                new PropertyMetadata((@this, _) => ((UniformGridBehavior)@this).SynchronizeDefinitions()));

        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }
        public static readonly DependencyProperty RowCountProperty
            = DependencyProperty.Register(
                nameof(RowCount),
                typeof(int),
                typeof(UniformGridBehavior),
                new PropertyMetadata((@this, _) => ((UniformGridBehavior)@this).SynchronizeDefinitions()));

        protected override void OnAttached()
        {
            base.OnAttached();

            SynchronizeDefinitions();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is not Grid grid)
                return;

            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
        }

        private void SynchronizeDefinitions()
        {
            if (AssociatedObject is not Grid grid)
                return;

            var columnDifference = grid.ColumnDefinitions.Count - ColumnCount;
            if (columnDifference > 0)
                grid.ColumnDefinitions.RemoveRange(0, columnDifference);
            else if (columnDifference < 0)
                foreach(var _ in Enumerable.Range(0, -columnDifference))
                    grid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });

            var rowDifference = grid.RowDefinitions.Count - RowCount;
            if (rowDifference > 0)
                grid.RowDefinitions.RemoveRange(0, rowDifference);
            else if (rowDifference < 0)
                foreach(var _ in Enumerable.Range(0, -rowDifference))
                    grid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });
        }
    }
}
