using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using Redux;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileViewModel
    {
        public SetupGamespaceBoardTileViewModel(
            SetupGamespaceBoardTileShipSegmentViewModelFactory shipSegmentFactory,
            IStore<GameStateModel> gameStateStore,
            Point position,
            IStore<ViewStateModel> viewStateStore)
        {
            Position = position;

            ReceiveShipSegmentMovement = ReactiveCommand.Create(
                viewStateStore
                    .Select(viewState => viewState.ActivePlayer)
                    .Where(activePlayer => activePlayer.HasValue)
                    .Select(activePlayer => activePlayer!.Value)
                    .DistinctUntilChanged()
                    .Select(activePlayer => new Action<ShipSegmentMovementModel>(model => gameStateStore.Dispatch(new MoveShipAction(
                        activePlayer,
                        model.ShipIndex,
                        model.Segment,
                        position)))));

            ShipSegments = gameStateStore
                .Select(gameState => gameState.Definition.Ships.Length)
                .DistinctUntilChanged()
                .Select(shipCount => Enumerable.Range(0, shipCount)
                    .Select(shipIndex => shipSegmentFactory.Create(
                        position,
                        shipIndex))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);
        }

        public Point Position { get; }

        public ICommand<ShipSegmentMovementModel> ReceiveShipSegmentMovement { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileShipSegmentViewModel>> ShipSegments { get; }
    }
}
