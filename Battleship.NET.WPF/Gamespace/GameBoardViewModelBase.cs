using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Windows;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardViewModelBase<TBoardPositionViewModel>
    {
        protected GameBoardViewModelBase(
            GameBoardColumnHeadingsViewModel columnHeadings,
            GameBoardRowHeadingsViewModel rowHeadings)
        {
            // TODO: Implement this
            BoardPositions = Observable.Never<ImmutableArray<TBoardPositionViewModel>>()
                .ToReactiveProperty();

            // TODO: Implement this
            BoardSize = Observable.Never<System.Drawing.Size>()
                .ToReactiveProperty();

            ColumnHeadings = columnHeadings;

            RowHeadings = rowHeadings;
        }

        public IReadOnlyObservableProperty<ImmutableArray<TBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }

        public GameBoardColumnHeadingsViewModel ColumnHeadings { get; }

        public GameBoardRowHeadingsViewModel RowHeadings { get; }
    }
}
