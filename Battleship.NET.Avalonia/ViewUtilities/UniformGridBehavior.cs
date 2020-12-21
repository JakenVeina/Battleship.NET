using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public class UniformGridBehavior
        : Behavior<Grid>
    {
        static UniformGridBehavior()
        {
            ColumnCountProperty.Changed.AddClassHandler<UniformGridBehavior>((@this, _) => @this.SynchronizeDefinitions());
            RowCountProperty.Changed.AddClassHandler<UniformGridBehavior>((@this, _) => @this.SynchronizeDefinitions());
        }

        public int ColumnCount
        {
            get => GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }
        public static readonly StyledProperty<int> ColumnCountProperty
            = AvaloniaProperty.Register<UniformGridBehavior, int>(nameof(ColumnCount));

        public int RowCount
        {
            get => GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }
        public static readonly StyledProperty<int> RowCountProperty
            = AvaloniaProperty.Register<UniformGridBehavior, int>(nameof(RowCount));

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
                grid.ColumnDefinitions.AddRange(Enumerable.Range(0, -columnDifference)
                    .Select(_ => new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    }));

            var rowDifference = grid.RowDefinitions.Count - RowCount;
            if (rowDifference > 0)
                grid.RowDefinitions.RemoveRange(0, rowDifference);
            else if (rowDifference < 0)
                grid.RowDefinitions.AddRange(Enumerable.Range(0, -rowDifference)
                    .Select(_ => new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    }));
        }
    }
}
