using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Windows;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardRowHeadingsViewModel
    {
        public GameBoardRowHeadingsViewModel()
        {
            // TODO: Implement this
            RowHeadings = Observable.Never<ImmutableArray<string>>()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<string>> RowHeadings { get; }
    }
}
