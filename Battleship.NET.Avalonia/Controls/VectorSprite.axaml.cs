using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship.NET.WPF.Controls
{
    public partial class VectorSprite
        : UserControl
    {
        public VectorSprite()
            => InitializeComponent();

        public VectorSpriteDefinition? Definition
        {
            get => (VectorSpriteDefinition?)GetValue(DefinitionProperty);
            set => SetValue(DefinitionProperty, value);
        }
        public static readonly DependencyProperty DefinitionProperty
            = DependencyProperty.Register(
                nameof(Definition),
                typeof(VectorSpriteDefinition),
                typeof(VectorSprite),
                new FrameworkPropertyMetadata(
                    defaultValue:   null,
                    flags:          FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush? Fill
        {
            get => (Brush?)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        public static readonly DependencyProperty FillProperty
            = DependencyProperty.Register(
                nameof(Fill),
                typeof(Brush),
                typeof(VectorSprite));
    }
}
