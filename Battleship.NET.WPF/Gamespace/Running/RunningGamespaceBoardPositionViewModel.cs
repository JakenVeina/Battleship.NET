using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceBoardPositionViewModel
        : GameBoardPositionViewModelBase
    {
        public RunningGamespaceBoardPositionViewModel(
                System.Drawing.Point position)
            : base(position)
        {
            // TODO: Implement this
            FireShotCommand = ReactiveCommand.Create(() => { });

            // TODO: Implement this
            ShipAsset = Observable.Never<ShipSegmentAssetModel?>()
                .ToReactiveProperty();

            // TODO: Implement this
            ShotOutcome = Observable.Never<ShotOutcome?>()
                .ToReactiveProperty();
        }

        public ICommand<Unit> FireShotCommand { get; }

        public IReadOnlyObservableProperty<ShipSegmentAssetModel?> ShipAsset { get; }

        public IReadOnlyObservableProperty<ShotOutcome?> ShotOutcome { get; }
    }
}
