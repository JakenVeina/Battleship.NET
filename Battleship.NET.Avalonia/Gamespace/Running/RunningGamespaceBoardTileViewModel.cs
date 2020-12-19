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

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceBoardTileViewModel
    {
        public RunningGamespaceBoardTileViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position)
        {
            Position = position;

            var shipSegmentModel = gameStateStore
                .Select(gameState =>
                (
                    gameBoard:          gameState.OpponentPlayerState.GameBoard,
                    shipDefinitions:    gameState.Definition.Ships
                ))
                .DistinctUntilChanged()
                .Select(model => Enumerable.Zip(
                        model.shipDefinitions,
                        model.gameBoard.Ships,
                        (definition, state) => (definition, state))
                    .SelectMany((shipModel, index) => shipModel.definition.Segments
                        .Select(segment =>
                        (
                            index:          index,
                            position:       segment
                                                .RotateOrigin(shipModel.state.Orientation)
                                                .Translate(shipModel.state.Position),
                            orientation:    shipModel.state.Orientation,
                            segment:        segment,
                            shipDefinition: shipModel.definition,
                            shipState:      shipModel.state
                        )))
                    .Where(shipSegmentModel => (shipSegmentModel.position == position)
                        && shipSegmentModel.shipState
                            .EnumerateSegmentPositions(shipSegmentModel.shipDefinition)
                            .All(segmentPosition => model.gameBoard.Hits.Contains(segmentPosition)))
                    .Select(shipSegmentModel => shipSegmentModel.ToNullable())
                    .FirstOrDefault())
                .DistinctUntilChanged();

            ShipAsset = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel.HasValue
                    ? new ShipSegmentAssetModel(
                        shipSegmentModel.Value.segment,
                        shipSegmentModel.Value.shipDefinition.Name)
                    : null)
                .DistinctUntilChanged();

            ShipOrientation = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel?.orientation)
                .DistinctUntilChanged();

            ShotAsset = gameStateStore
                .Select(gameState => gameState.OpponentPlayerState.GameBoard)
                .DistinctUntilChanged()
                .Select(gameBoard => gameBoard switch
                {
                    _ when gameBoard.Hits.Contains(position)    => ShotAssetModel.Hit,
                    _ when gameBoard.Misses.Contains(position)  => ShotAssetModel.Miss,
                    _                                           => null
                })
                .DistinctUntilChanged();

            FireShot = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new FireShotAction(position)),
                gameStateStore
                    .Select(gameState => gameState.CanFireShot(position))
                    .DistinctUntilChanged());
        }

        public Point Position { get; }

        public IObservable<ShipSegmentAssetModel?> ShipAsset { get; }
        
        public IObservable<Rotation?> ShipOrientation { get; }

        public IObservable<ShotAssetModel?> ShotAsset { get; }

        public ICommand<Unit> FireShot { get; }
    }
}
