using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileShipSegmentView : UserControl
    {
        public SetupGamespaceBoardTileShipSegmentView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
