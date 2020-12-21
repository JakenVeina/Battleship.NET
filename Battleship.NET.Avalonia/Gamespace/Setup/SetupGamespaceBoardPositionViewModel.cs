using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardPositionViewModel
    {
        public SetupGamespaceBoardPositionViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position,
            IStore<ViewStateModel> viewStateStore)
        {
            Position = position;

            ReceiveShipSegment = ReactiveCommand.Create(
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

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegment { get; }
    }
}
