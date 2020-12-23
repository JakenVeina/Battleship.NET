using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using ReduxSharp;

using Battleship.NET.Domain;
using Battleship.NET.Domain.Behaviors;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Game;
using Battleship.NET.WPF.Gamespace.Completed;
using Battleship.NET.WPF.Gamespace.Idle;
using Battleship.NET.WPF.Gamespace.Ready;
using Battleship.NET.WPF.Gamespace.Running;
using Battleship.NET.WPF.Gamespace.Setup;
using Battleship.NET.WPF.Player;
using Battleship.NET.WPF.State;
using Battleship.NET.WPF.State.Behaviors;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF
{
    public partial class Application
        : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _serviceProvider = new ServiceCollection()
                    .AddSingleton<ISystemClock, DefaultSystemClock>()
                    .AddSingleton<Random>()
                    .AddSingleton<IStore<GameStateModel>, Store<GameStateModel>>(_ => new Store<GameStateModel>(new GameStateReducer(), StandardGame.CreateIdle()))
                    .AddTransient<IBehavior, GameCompletionBehavior>()
                    .AddTransient<IBehavior, GameClockBehavior>()
                    .AddSingleton<IStore<ViewStateModel>, Store<ViewStateModel>>(_ => new Store<ViewStateModel>(new ViewStateReducer(), ViewStateModel.Default))
                    .AddTransient<IBehavior, ActivePlayerSynchronizationBehavior>()
                    .AddTransient<GameViewModel>()
                    .AddTransient<CompletedGamespaceBoardPositionViewModelFactory>()
                    .AddTransient<CompletedGamespaceViewModel>()
                    .AddTransient<IdleGamespaceViewModel>()
                    .AddTransient<ReadyGamespaceViewModel>()
                    .AddTransient<RunningGamespaceBoardPositionViewModelFactory>()
                    .AddTransient<RunningGamespaceViewModel>()
                    .AddSingleton<SetupGamespaceBoardPositionViewModelFactory>()
                    .AddSingleton<SetupGamespaceShipSegmentViewModelFactory>()
                    .AddTransient<SetupGamespaceViewModel>()
                    .AddSingleton<PlayerViewModelFactory>()
                    .BuildServiceProvider();

            _behaviorStopTokens = _serviceProvider.GetServices<IBehavior>()
                .Select(behavior => behavior.Start(new SynchronizationContextScheduler(SynchronizationContext.Current!)))
                .ToImmutableArray();

            _gameWindow = new GameWindow()
            {
                DataContext = _serviceProvider.GetRequiredService<GameViewModel>()
            };

            _gameWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _gameWindow?.Close();

            if (_behaviorStopTokens is not null)
                foreach (var token in _behaviorStopTokens)
                    token.Dispose();

            _serviceProvider?.Dispose();
        }

        private ImmutableArray<IDisposable>? _behaviorStopTokens;
        private GameWindow? _gameWindow;
        private ServiceProvider? _serviceProvider;
    }
}
