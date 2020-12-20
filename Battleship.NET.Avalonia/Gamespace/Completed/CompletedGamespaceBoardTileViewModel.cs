using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;

using Redux;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Avalonia.Gamespace.Running;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceBoardTileViewModel
    {
        public CompletedGamespaceBoardTileViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position,
            IStore<ViewStateModel> viewStateStore)
        {
            Position = position;

            var model = Observable.CombineLatest(
                    gameStateStore,
                    viewStateStore,
                    (gameState, viewState) => 
                    (
                        activePlayer:       viewState.ActivePlayer,
                        player1GameBoard:   gameState.Player1.GameBoard,
                        player2GameBoard:   gameState.Player2.GameBoard,
                        shipDefinitions:    gameState.Definition.Ships
                    ))
                .Where(model => model.activePlayer.HasValue)
                .Select(model => 
                (
                    gameBoard:          (model.activePlayer == GamePlayer.Player1)
                                            ? model.player1GameBoard
                                            : model.player2GameBoard,
                    shipDefinitions:    model.shipDefinitions
                ))
                .ShareReplayDistinct(1);

            var shipSegmentModel = model
                .Select(model => Enumerable.Zip(
                        model.shipDefinitions,
                        model.gameBoard.Ships,
                        (definition, state) => (definition, state))
                    .SelectMany((shipModel, index) => shipModel.definition.Segments
                        .Select(segment =>
                        (
                            gameBoardHits:  model.gameBoard.Hits,
                            index:          index,
                            orientation:    shipModel.state.Orientation,
                            position:       segment
                                                .RotateOrigin(shipModel.state.Orientation)
                                                .Translate(shipModel.state.Position),
                            segment:        segment,
                            shipDefinition: shipModel.definition,
                            shipState:      shipModel.state
                        )))
                    .Where(shipSegmentModel => (shipSegmentModel.position == position))
                    .Select(shipSegmentModel => shipSegmentModel.ToNullable())
                    .FirstOrDefault())
                .ShareReplayDistinct(1);

            IsShipSunk = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel.HasValue
                    && shipSegmentModel.Value.shipState
                        .EnumerateSegmentPositions(shipSegmentModel.Value.shipDefinition)
                        .All(segmentPosition => shipSegmentModel.Value.gameBoardHits.Contains(segmentPosition)))
                .ShareReplayDistinct(1);

            ShipAsset = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel.HasValue
                    ? new ShipSegmentAssetModel(
                        shipSegmentModel.Value.segment,
                        shipSegmentModel.Value.shipDefinition.Name)
                    : null)
                .ShareReplayDistinct(1);

            ShipOrientation = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel?.orientation)
                .ShareReplayDistinct(1);

            ShotAsset = model
                .Select(model => model switch
                {
                    _ when model.gameBoard.Hits.Contains(position)      => ShotAssetModel.Hit,
                    _ when model.gameBoard.Misses.Contains(position)    => ShotAssetModel.Miss,
                    _                                                   => null
                })
                .ShareReplayDistinct(1);
        }

        public Point Position { get; }

        public IObservable<bool> IsShipSunk { get; }

        public IObservable<ShipSegmentAssetModel?> ShipAsset { get; }

        public IObservable<Rotation?> ShipOrientation { get; }

        public IObservable<ShotAssetModel?> ShotAsset { get; }
    }
}
