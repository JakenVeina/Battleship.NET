using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardRowHeadingsViewModel
    {
        public GameBoardRowHeadingsViewModel(
            IStore<GameStateModel> gameStateStore)
        {
            RowHeadings = gameStateStore
                .Select(BoardSelectors.RowHeadings)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<string>> RowHeadings { get; }
    }
}
