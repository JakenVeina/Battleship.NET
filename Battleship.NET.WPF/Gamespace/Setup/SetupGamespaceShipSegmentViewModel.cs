using System;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

using Battleship.NET.WPF.Ship;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentViewModel
    {
        public SetupGamespaceShipSegmentViewModel(
            System.Drawing.Point segment,
            int shipIndex)
        {
            // TODO: Implement this
            Asset = Observable.Never<ShipSegmentAssetModel>()
                .ToReactiveProperty();

            // TODO: Implement this
            IsValid = Observable.Never<bool>()
                .ToReactiveProperty();

            // TODO: Implement this
            Position = Observable.Never<System.Drawing.Point?>()
                .ToReactiveProperty();

            // TODO: Implement this
            ReceiveShipSegmentCommand = ReactiveCommand.Create(Observable.Return<Action<ShipSegmentAssetModel>>(_ => { }));

            // TODO: Implement this
            RotateCommand = ReactiveCommand.Create(() => { });
        }

        public IReadOnlyObservableProperty<ShipSegmentAssetModel> Asset { get; }

        public IReadOnlyObservableProperty<bool> IsValid { get; }

        public IReadOnlyObservableProperty<System.Drawing.Point?> Position { get; }

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegmentCommand { get; }
        
        public ICommand<Unit> RotateCommand { get; }
    }
}
