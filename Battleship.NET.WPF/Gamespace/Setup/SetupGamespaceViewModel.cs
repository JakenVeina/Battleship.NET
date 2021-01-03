using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceViewModel
        : GameBoardViewModelBase<SetupGamespaceBoardPositionViewModel>
    {
        public SetupGamespaceViewModel(
                GameBoardColumnHeadingsViewModel columnHeadings,
                GameBoardRowHeadingsViewModel rowHeadings)
            : base(
                columnHeadings,
                rowHeadings)
        {
            // TODO: Implement this
            CompleteSetupCommand = ReactiveCommand.Create(() => { });

            // TODO: Implement this
            RandomizeShipsCommand = ReactiveCommand.Create(() => { });

            // TODO: Implement this
            ShipSegments = Observable.Never<ImmutableArray<SetupGamespaceShipSegmentViewModel>>()
                .ToReactiveProperty();
        }

        public ICommand<Unit> CompleteSetupCommand { get; }

        public ICommand<Unit> RandomizeShipsCommand { get; }

        public IReadOnlyObservableProperty<ImmutableArray<SetupGamespaceShipSegmentViewModel>> ShipSegments { get; }
    }
}
