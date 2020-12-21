using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionView
        : UserControl
    {
        public CompletedGamespaceBoardPositionView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
