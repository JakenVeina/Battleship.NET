using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Idle
{
    public class IdleGamespaceView
        : UserControl
    {
        public IdleGamespaceView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
