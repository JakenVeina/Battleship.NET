using System.Reactive.Linq;
using System.Windows;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionViewModel
        : GameBoardPositionViewModelBase
    {
        public CompletedGamespaceBoardPositionViewModel(
                System.Drawing.Point position)
            : base(position)
        {
            // TODO: Implement this
            IsShipSunk = Observable.Never<bool>()
                .ToReactiveProperty();

            // TODO: Implement this
            ShipAsset = Observable.Never<ShipSegmentAssetModel?>()
                .ToReactiveProperty();

            // TODO: Implement this
            ShotOutcome = Observable.Never<ShotOutcome?>()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsShipSunk { get; }
        
        public IReadOnlyObservableProperty<ShipSegmentAssetModel?> ShipAsset { get; }

        public IReadOnlyObservableProperty<ShotOutcome?> ShotOutcome { get; }
    }
}
