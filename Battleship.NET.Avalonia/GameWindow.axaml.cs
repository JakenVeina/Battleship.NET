using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;

using Microsoft.Extensions.DependencyInjection;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

using Battleship.NET.Avalonia.Game;
using Battleship.NET.Domain.Behaviors;

namespace Battleship.NET.Avalonia
{
    public class GameWindow
        : Window
    {
        public GameWindow(
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _behaviorStopTokens = serviceProvider.GetServices<IBehavior>()
                .Select(behavior => behavior.Start(AvaloniaScheduler.Instance))
                .ToImmutableArray();

            DataContext = serviceProvider.GetRequiredService<GameViewModel>();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            foreach (var token in _behaviorStopTokens)
                token.Dispose();
        }

        private readonly ImmutableArray<IDisposable> _behaviorStopTokens;
    }
}
