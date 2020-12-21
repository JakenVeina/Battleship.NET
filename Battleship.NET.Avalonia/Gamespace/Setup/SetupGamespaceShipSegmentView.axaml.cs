using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentView
        : UserControl
    {
        public SetupGamespaceShipSegmentView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
