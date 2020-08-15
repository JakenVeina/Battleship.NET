using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;

namespace Battleship.NET.Avalonia.ViewUtilities
{
    public class ClassSelectorBehavior
        : Behavior<IStyledElement>
    {
        static ClassSelectorBehavior()
        {
            DataProperty.Changed.AddClassHandler<ClassSelectorBehavior>((@this, _) => @this.MapClasses());
        }

        public object? Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public static readonly StyledProperty<object?> DataProperty
            = AvaloniaProperty.Register<ClassSelectorBehavior, object?>(nameof(Data));

        [Content]
        public AvaloniaList<ClassMapping> Mappings { get; }
            = new AvaloniaList<ClassMapping>();

        protected override void OnAttached()
        {
            base.OnAttached();

            MapClasses();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            foreach (var mapping in Mappings)
                if (mapping.Class is { })
                    AssociatedObject!.Classes.Remove(mapping.Class);
        }

        private void MapClasses()
        {
            if (AssociatedObject is null)
                return;

            var data = Data;

            foreach (var mapping in Mappings)
            {
                if (mapping.Class is null)
                    continue;

                if (AreEqual(data, mapping.Value))
                    AssociatedObject.Classes.Add(mapping.Class);
                else
                    AssociatedObject.Classes.Remove(mapping.Class);
            }
        }

        private bool AreEqual(object? dataValue, object? mappingValue)
            => ((dataValue is null) || (mappingValue is null))
                ? Equals(dataValue, mappingValue)
                : Equals(dataValue, TypeConverterHelper.Convert(mappingValue.ToString()!, dataValue.GetType()));
    }
}
