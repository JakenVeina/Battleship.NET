using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Input;

using Battleship.NET.WPF.Ship;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceBoardPositionViewModel
        : GameBoardPositionViewModelBase
    {
        public SetupGamespaceBoardPositionViewModel(
                Point position)
            : base(position)
        {
            // TODO: Implement this
            ReceiveShipSegmentCommand = ReactiveCommand.Create(Observable.Return<Action<ShipSegmentAssetModel>>(_ => { }));
        }

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegmentCommand { get; }
    }
}
