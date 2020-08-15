using System;

using Microsoft.Extensions.DependencyInjection;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Battleship.NET.Avalonia.Game;

namespace Battleship.NET.Avalonia
{
    public class GameWindow
        : Window
    {
        public GameWindow(
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = serviceProvider.GetRequiredService<GameViewModel>();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
