using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Resources
{
    public class SharedSpriteDefinitions
        : ResourceDictionary
    {
        public SharedSpriteDefinitions()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
