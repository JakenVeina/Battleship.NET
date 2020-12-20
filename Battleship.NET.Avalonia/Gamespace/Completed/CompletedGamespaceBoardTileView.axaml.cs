using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceBoardTileView
        : UserControl
    {
        public CompletedGamespaceBoardTileView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
