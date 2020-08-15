using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Player
{
    public class PlayerView
        : UserControl
    {
        public PlayerView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
