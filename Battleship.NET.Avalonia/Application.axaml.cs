using System;
using System.Reactive.PlatformServices;

using Microsoft.Extensions.DependencyInjection;

using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Redux;

using Battleship.NET.Avalonia.Game;
using Battleship.NET.Avalonia.Gamespace.Idle;
using Battleship.NET.Avalonia.Gamespace.Paused;
using Battleship.NET.Avalonia.Gamespace.Ready;
using Battleship.NET.Avalonia.Gamespace.Running;
using Battleship.NET.Avalonia.Gamespace.Setup;
using Battleship.NET.Avalonia.Player;
using Battleship.NET.Domain;
using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Behaviors;

namespace Battleship.NET.Avalonia
{
    public class Application
        : global::Avalonia.Application
    {
        public override void Initialize()
            => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<ISystemClock, DefaultSystemClock>()
                    .AddSingleton<Random>()
                    .AddSingleton<IStore<GameStateModel>, StandardGame>()
                    .AddTransient<GameViewModel>()
                    .AddTransient<IdleGamespaceViewModel>()
                    .AddTransient<PausedGamespaceViewModel>()
                    .AddTransient<ReadyGamespaceViewModel>()
                    .AddTransient<RunningGamespaceViewModel>()
                    .AddTransient<RunningGamespaceBoardTileViewModelFactory>()
                    .AddTransient<SetupGamespaceViewModel>()
                    .AddSingleton<SetupGamespaceBoardTileViewModelFactory>()
                    .AddSingleton<SetupGamespaceBoardTileShipSegmentViewModelFactory>()
                    .AddSingleton<PlayerViewModelFactory>()
                    .AddTransient<IBehavior, GameCompletionBehavior>()
                    .AddTransient<IBehavior, GameClockBehavior>()
                    .BuildServiceProvider();

                desktopLifetime.Exit += (_, _) => serviceProvider.Dispose();

                desktopLifetime.MainWindow = new GameWindow(serviceProvider);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
