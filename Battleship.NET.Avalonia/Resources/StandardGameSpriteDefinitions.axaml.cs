using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Resources
{
    public class StandardGameSpriteDefinitions
        : ResourceDictionary
    {
        public StandardGameSpriteDefinitions()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
