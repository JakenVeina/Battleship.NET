using System.Windows;

using Microsoft.Xaml.Behaviors;

namespace Battleship.NET.WPF.ViewUtilities
{
    public class DragEnterTrigger
        : TriggerBase<UIElement>
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
                typeof(DragEnterTrigger));

        public DragDropEffects Effects
        {
            get => (DragDropEffects)GetValue(EffectsProperty);
            set => SetValue(EffectsProperty, value);
        }
        public static readonly DependencyProperty EffectsProperty
            = DependencyProperty.Register(
                nameof(Effects),
                typeof(DragDropEffects),
                typeof(DragEnterTrigger));

        protected override void OnAttached()
        {
            base.OnAttached();

            DragDrop.AddDragEnterHandler(AssociatedObject, OnDragEnter);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            DragDrop.RemoveDragEnterHandler(AssociatedObject, OnDragEnter);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (((e.Effects & Effects) == DragDropEffects.None)
                    || (DataFormat is null))
                return;

            var data = e.Data.GetData(DataFormat);

            InvokeActions(data);
        }
    }
}
