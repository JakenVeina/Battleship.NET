using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceBoardPositionView
        : UserControl
    {
        public RunningGamespaceBoardPositionView()
            => InitializeComponent();

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
