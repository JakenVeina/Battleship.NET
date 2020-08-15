using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Battleship.NET.Avalonia.Controls
{
    public class VectorSprite
        : UserControl
    {
        public VectorSprite()
            => InitializeComponent();

        public VectorSpriteDefinition Definition
        {
            get => GetValue(DefinitionProperty);
            set => SetValue(DefinitionProperty, value);
        }
        public static readonly StyledProperty<VectorSpriteDefinition> DefinitionProperty
            = AvaloniaProperty.Register<VectorSprite, VectorSpriteDefinition>(nameof(Definition));

        public IBrush Fill
        {
            get => GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        public static readonly StyledProperty<IBrush> FillProperty
            = AvaloniaProperty.Register<VectorSprite, IBrush>(nameof(Fill));

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
