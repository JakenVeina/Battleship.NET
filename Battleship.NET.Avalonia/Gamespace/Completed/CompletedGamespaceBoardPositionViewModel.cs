using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionViewModel
    {
        public CompletedGamespaceBoardPositionViewModel(
            IStore<GameStateModel> gameStateStore,
            System.Drawing.Point position,
            IStore<ViewStateModel> viewStateStore)
        {
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
                .ToReactiveProperty();

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
                .ToReactiveProperty();

            IsShipSunk = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel.HasValue
                    && shipSegmentModel.Value.shipState
                        .EnumerateSegmentPositions(shipSegmentModel.Value.shipDefinition)
                        .All(segmentPosition => shipSegmentModel.Value.gameBoardHits.Contains(segmentPosition)))
                .ToReactiveProperty();

            Position = position;

            ShipAsset = shipSegmentModel
                .Select(shipSegmentModel => shipSegmentModel.HasValue
                    ? new ShipSegmentAssetModel(
                        shipSegmentModel.Value.index,
                        shipSegmentModel.Value.shipDefinition.Name,
                        shipSegmentModel.Value.orientation,
                        shipSegmentModel.Value.segment)
                    : null)
                .ToReactiveProperty();

            ShotAsset = model
                .Select(model => model switch
                {
                    _ when model.gameBoard.Hits.Contains(position)      => ShotAssetModel.Hit,
                    _ when model.gameBoard.Misses.Contains(position)    => ShotAssetModel.Miss,
                    _                                                   => null
                })
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsShipSunk { get; }
        
        public System.Drawing.Point Position { get; }

        public IReadOnlyObservableProperty<ShipSegmentAssetModel?> ShipAsset { get; }

        public IReadOnlyObservableProperty<ShotAssetModel?> ShotAsset { get; }
    }
}
