using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using Redux;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileViewModel
    {
        public SetupGamespaceBoardTileViewModel(
            SetupGamespaceBoardTileShipSegmentViewModelFactory gameBoardTileShipSegmentViewModelFactory,
            IStore<GameStateModel> gameStateStore,
            Point position)
        {
            Position = position;

            var currentPlayer = gameStateStore
                .Select(gameState => gameState.CurrentPlayer)
                .DistinctUntilChanged();

            ReceiveShipSegmentMovement = ReactiveCommand.Create(
                currentPlayer
                    .Select(currentPlayer => new Action<ShipSegmentMovementModel>(model => gameStateStore.Dispatch(new MoveShipAction(
                        currentPlayer,
                        model.ShipIndex,
                        model.Segment,
                        position)))));

            ShipSegments = gameStateStore
                .Select(gameState => gameState.Definition.Ships.Length)
                .DistinctUntilChanged()
                .Select(shipCount => Enumerable.Range(0, shipCount)
                    .Select(shipIndex => gameBoardTileShipSegmentViewModelFactory.Create(
                        position,
                        shipIndex))
                    .ToImmutableArray())
                .DistinctUntilChanged();
        }

        public Point Position { get; }

        public ICommand<ShipSegmentMovementModel> ReceiveShipSegmentMovement { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileShipSegmentViewModel>> ShipSegments { get; }
    }
}
