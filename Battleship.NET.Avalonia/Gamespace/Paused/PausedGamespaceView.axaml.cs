using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Paused
{
    public class PausedGamespaceView
        : UserControl
    {
        public PausedGamespaceView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
