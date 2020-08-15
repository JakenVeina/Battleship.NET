using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public class DragEnterTriggerBehavior
        : Trigger<InputElement>
    {
        public string? DataFormat
        {
            get => GetValue(DataFormatProperty);
            set => SetValue(DataFormatProperty, value);
        }
        public static readonly StyledProperty<string?> DataFormatProperty
            = AvaloniaProperty.Register<ClickAndDragBehavior, string?>(nameof(DataFormat));

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

            AssociatedObject!.AddHandler(DragDrop.DragEnterEvent, OnDragEnter!);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject!.RemoveHandler(DragDrop.DragEnterEvent, OnDragEnter!);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if ((e.DragEffects & Effects) == DragDropEffects.None)
                return;

            var dataValue = e.Data.Get(DataFormat);

            foreach (var action in Actions!)
                (action as IAction)?.Execute(sender, dataValue);
        }
    }
}
