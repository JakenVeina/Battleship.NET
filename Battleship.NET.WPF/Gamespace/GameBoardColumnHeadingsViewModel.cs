using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Windows;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardColumnHeadingsViewModel
    {
        public GameBoardColumnHeadingsViewModel()
        {
            // TODO: Implement this
            ColumnHeadings = Observable.Never<ImmutableArray<string>>()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<string>> ColumnHeadings { get; }
    }
}
