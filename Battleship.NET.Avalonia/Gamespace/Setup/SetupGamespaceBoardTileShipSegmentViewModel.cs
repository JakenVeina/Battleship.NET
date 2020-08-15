using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using Redux;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileShipSegmentViewModel
    {
        public SetupGamespaceBoardTileShipSegmentViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position,
            int shipIndex)
        {
            var segmentModel = gameStateStore
                .Select(gameState =>
                (
                    currentPlayer:  gameState.CurrentPlayer,
                    definition:     gameState.Definition.Ships[shipIndex],
                    state:          gameState.CurrentPlayerState.GameBoard.Ships[shipIndex]
                ))
                .DistinctUntilChanged()
                .Select(model => model.definition.Segments
                    .Select(segment => 
                    (
                        currentPlayer:  model.currentPlayer,
                        name:           model.definition.Name,
                        orientation:    model.state.Orientation,
                        position:       segment
                                            .RotateOrigin(model.state.Orientation)
                                            .Translate(model.state.Position),
                        segment:        segment
                    ).ToNullable())
                    .FirstOrDefault(model => model!.Value.position == position))
                .DistinctUntilChanged();

            Asset = segmentModel
                .Select(model => model.HasValue
                    ? new ShipSegmentAssetModel(
                        model.Value.segment,
                        model.Value.name)
                    : null)
                .DistinctUntilChanged();

            HasShip = segmentModel
                .Select(model => model.HasValue)
                .DistinctUntilChanged();

            IsShipValid = gameStateStore
                .Select(gameState =>
                (
                    shipDefinitions: gameState.Definition.Ships,
                    ShipStates: gameState.CurrentPlayerState.GameBoard.Ships
                ))
                .DistinctUntilChanged()
                .Select(model => !Enumerable.Zip(
                        model.shipDefinitions,
                        model.ShipStates,
                        (definition, state) => (definition, state))
                    .SelectMany((ship, index) => ship.definition.Segments
                        .Select(segment =>
                        (
                            shipIndex: index,
                            position: segment
                                .RotateOrigin(ship.state.Orientation)
                                .Translate(ship.state.Position)
                        )))
                    .GroupBy(x => x.position)
                    .Any(group => group.Any(x => x.shipIndex == shipIndex)
                        && (group.Count() > 1)))
                .DistinctUntilChanged();

            Movement = segmentModel
                .Select(model => model.HasValue
                    ? new ShipSegmentMovementModel(
                        model.Value.segment,
                        shipIndex)
                    : null)
                .DistinctUntilChanged();

            Orientation = segmentModel
                .Select(model => model?.orientation)
                .DistinctUntilChanged();

            Rotate = ReactiveCommand.Create(
                segmentModel
                    .Where(model => model.HasValue)
                    .Select(model => new Action(() => gameStateStore.Dispatch(new RotateShipAction(
                        model!.Value.currentPlayer,
                        shipIndex,
                        model!.Value.segment,
                        model!.Value.orientation switch
                        {
                            Rotation.Rotate0    => Rotation.Rotate90,
                            Rotation.Rotate90   => Rotation.Rotate180,
                            Rotation.Rotate180  => Rotation.Rotate270,
                            _                   => Rotation.Rotate0
                        })))),
                HasShip);
        }

        public IObservable<ShipSegmentAssetModel?> Asset { get; }

        public IObservable<bool> HasShip { get; }

        public IObservable<bool> IsShipValid { get; }

        public IObservable<ShipSegmentMovementModel?> Movement { get; }

        public IObservable<Rotation?> Orientation { get; }

        public ICommand<Unit> Rotate { get; }
    }
}
