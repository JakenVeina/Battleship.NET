using System;

using Microsoft.Extensions.DependencyInjection;

using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Redux;

using Battleship.NET.Avalonia.Game;
using Battleship.NET.Avalonia.Gamespace.Idle;
using Battleship.NET.Avalonia.Gamespace.Paused;
using Battleship.NET.Avalonia.Gamespace.Ready;
using Battleship.NET.Avalonia.Gamespace.Setup;
using Battleship.NET.Avalonia.Player;
using Battleship.NET.Domain;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia
{
    public class Application
        : global::Avalonia.Application
    {
        public override void Initialize()
            => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<Random>()
                    .AddSingleton<IStore<GameStateModel>, StandardGame>()
                    .AddTransient<GameViewModel>()
                    .AddTransient<IdleGamespaceViewModel>()
                    .AddTransient<PausedGamespaceViewModel>()
                    .AddTransient<ReadyGamespaceViewModel>()
                    .AddTransient<SetupGamespaceViewModel>()
                    .AddSingleton<SetupGamespaceBoardTileViewModelFactory>()
                    .AddSingleton<SetupGamespaceBoardTileShipSegmentViewModelFactory>()
                    .AddSingleton<PlayerViewModelFactory>()
                    .BuildServiceProvider();

                desktop.Exit += (_, _) => serviceProvider.Dispose();

                desktop.MainWindow = new GameWindow(serviceProvider);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
