using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceBoardTileView
        : UserControl
    {
        public RunningGamespaceBoardTileView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
