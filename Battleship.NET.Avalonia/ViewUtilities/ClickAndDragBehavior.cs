using System.Windows;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace Battleship.NET.WPF.ViewUtilities
{
    public class ClickAndDragBehavior
        : Behavior<UIElement>
    {
        public string? DataFormat
        {
            get => (string?)GetValue(DataFormatProperty);
            set => SetValue(DataFormatProperty, value);
        }
        public static readonly DependencyProperty DataFormatProperty
            = DependencyProperty.Register(
                nameof(DataFormat),
                typeof(string),
                typeof(ClickAndDragBehavior));

        public object? DataValue
        {
            get => GetValue(DataValueProperty);
            set => SetValue(DataValueProperty, value);
        }
        public static readonly DependencyProperty DataValueProperty
            = DependencyProperty.Register(
                nameof(DataValue),
                typeof(object),
                typeof(ClickAndDragBehavior));

        public DragDropEffects Effects
        {
            get => (DragDropEffects)GetValue(EffectsProperty);
            set => SetValue(EffectsProperty, value);
        }
        public static readonly DependencyProperty EffectsProperty
            = DependencyProperty.Register(
                nameof(Effects),
                typeof(DragDropEffects),
                typeof(ClickAndDragBehavior));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseMove += OnMouseMove;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseMove -= OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //if ((e.MouseDevice.Captured is null)
            if ((e.LeftButton != MouseButtonState.Pressed)
                    || (DataFormat is null)
                    || (DataValue is null))
                return;

            var dataObject = new DataObject();
            dataObject.SetData(DataFormat, DataValue);

            DragDrop.DoDragDrop(AssociatedObject, dataObject, Effects);
        }
    }
}
