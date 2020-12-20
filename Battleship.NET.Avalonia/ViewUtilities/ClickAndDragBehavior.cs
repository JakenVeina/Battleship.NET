using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public class ClickAndDragBehavior
        : Behavior<InputElement>
    {
        public bool AllowDrag
        {
            get => GetValue(AllowDragProperty);
            set => SetValue(AllowDragProperty, value);
        }
        public static readonly StyledProperty<bool> AllowDragProperty
            = AvaloniaProperty.Register<ClickAndDragBehavior, bool>(nameof(AllowDrag));

        public string? DataFormat
        {
            get => GetValue(DataFormatProperty);
            set => SetValue(DataFormatProperty, value);
        }
        public static readonly StyledProperty<string?> DataFormatProperty
            = AvaloniaProperty.Register<ClickAndDragBehavior, string?>(nameof(DataFormat));

        public object? DataValue
        {
            get => GetValue(DataValueProperty);
            set => SetValue(DataValueProperty, value);
        }
        public static readonly StyledProperty<object?> DataValueProperty
            = AvaloniaProperty.Register<ClickAndDragBehavior, object?>(nameof(DataValue));

        public DragDropEffects Effects
        {
            get => GetValue(EffectsProperty);
            set => SetValue(EffectsProperty, value);
        }
        public static readonly StyledProperty<DragDropEffects> EffectsProperty
            = AvaloniaProperty.Register<ClickAndDragBehavior, DragDropEffects>(nameof(Effects));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject!.PointerMoved += OnPointerMoved!;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject!.PointerMoved -= OnPointerMoved!;
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            if ((e.Pointer.Captured is null)
                    || !AllowDrag
                    || (DataFormat is null)
                    || (DataValue is null))
                return;

            var dataObject = new DataObject();
            dataObject.Set(DataFormat, DataValue);

            DragDrop.DoDragDrop(e, dataObject, Effects);
        }
    }
}
