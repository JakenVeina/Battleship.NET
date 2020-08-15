using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceView
        : UserControl
    {
        public SetupGamespaceView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
