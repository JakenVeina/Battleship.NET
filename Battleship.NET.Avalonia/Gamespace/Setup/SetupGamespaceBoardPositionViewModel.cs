using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceBoardPositionViewModel
    {
        public SetupGamespaceBoardPositionViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position,
            IStore<ViewStateModel> viewStateStore)
        {
            Position = position;

            ReceiveShipSegmentCommand = ReactiveCommand.Create(
                viewStateStore
                    .Select(viewState => viewState.ActivePlayer)
                    .Where(activePlayer => activePlayer.HasValue)
                    .Select(activePlayer => activePlayer!.Value)
                    .DistinctUntilChanged()
                    .Select(activePlayer => new Action<ShipSegmentAssetModel>(asset => gameStateStore.Dispatch(new MoveShipAction(
                        activePlayer,
                        asset.Index,
                        asset.Segment,
                        position)))));
        }

        public Point Position { get; }

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegmentCommand { get; }
    }
}
