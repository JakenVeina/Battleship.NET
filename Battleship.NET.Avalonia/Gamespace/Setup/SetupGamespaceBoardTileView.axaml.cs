using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileView
        : UserControl
    {
        public SetupGamespaceBoardTileView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
