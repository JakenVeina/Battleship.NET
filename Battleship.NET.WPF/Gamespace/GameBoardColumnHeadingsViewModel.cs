using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardColumnHeadingsViewModel
    {
        public GameBoardColumnHeadingsViewModel(
            IStore<GameStateModel> gameStateStore)
        {
            ColumnHeadings = gameStateStore
                .Select(BoardSelectors.ColumnHeadings)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<string>> ColumnHeadings { get; }
    }
}
