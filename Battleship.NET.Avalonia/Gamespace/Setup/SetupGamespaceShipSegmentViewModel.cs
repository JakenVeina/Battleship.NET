using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentViewModel
    {
        public SetupGamespaceShipSegmentViewModel(
            IStore<GameStateModel> gameStateStore,
            Point segment,
            int shipIndex,
            IStore<ViewStateModel> viewStateStore)
        {
            Asset = gameStateStore
                .Select(gameState => gameState.Definition)
                .DistinctUntilChanged()
                .Select(definition => new ShipSegmentAssetModel(
                    shipIndex,
                    definition.Ships[shipIndex].Name,
                    segment))
                .ShareReplayDistinct(1);

            var model = Observable.CombineLatest(
                    gameStateStore,
                    viewStateStore,
                    (gameState, viewState) =>
                    (
                        activePlayer:       viewState.ActivePlayer,
                        boardPositions:     gameState.Definition.GameBoard.Positions,
                        player1BoardState:  gameState.Player1.GameBoard,
                        player2BoardState:  gameState.Player2.GameBoard,
                        shipDefinitions:    gameState.Definition.Ships
                    ))
                .Where(model => model.activePlayer.HasValue)
                .Select(model =>
                (
                    activePlayer:       model.activePlayer!.Value,
                    boardPositions:     model.boardPositions,
                    boardState:         (model.activePlayer == GamePlayer.Player1)
                                            ? model.player1BoardState
                                            : model.player2BoardState,
                    shipDefinitions:    model.shipDefinitions
                ))
                .ShareReplayDistinct(1);

            var segmentModel = model
                .Select(model => 
                (
                    activePlayer:       model.activePlayer,
                    boardPositions:     model.boardPositions,
                    state:              model.boardState.Ships[shipIndex]
                ))
                .ShareReplayDistinct(1);

            Orientation = segmentModel
                .Select(segmentModel => segmentModel.state.Orientation)
                .ShareReplayDistinct(1);

            Position = segmentModel
                .Select(segmentModel => 
                (
                    boardPositions: segmentModel.boardPositions,
                    position:       segment
                                .RotateOrigin(segmentModel.state.Orientation)
                                .Translate(segmentModel.state.Position)
                ))
                .Select(model => model.boardPositions.Contains(model.position)
                    ? model.position.ToNullable()
                    : null)
                .ShareReplayDistinct(1);

            IsValid = model
                .DistinctUntilChanged()
                .Select(model => model.boardState.Ships[shipIndex]
                    .EnumerateSegmentPositions(model.shipDefinitions[shipIndex])
                    .All(shipPosition => model.boardPositions.Contains(shipPosition)
                        && Enumerable.Zip(
                                model.shipDefinitions,
                                model.boardState.Ships,
                                (definition, state) => (definition, state))
                            .Where((_, index) => index != shipIndex)
                            .SelectMany(ship => ship.state
                                .EnumerateSegmentPositions(ship.definition))
                            .All(position => position != shipPosition)))
                .ShareReplayDistinct(1);

            ReceiveShipSegmentCommand = ReactiveCommand.Create(
                execute:    Observable.CombineLatest(
                    viewStateStore
                        .Select(viewState => viewState.ActivePlayer)
                        .Where(activePlayer => activePlayer.HasValue)
                        .Select(activePlayer => activePlayer!.Value)
                        .DistinctUntilChanged(),
                    Position,
                    (activePlayer, position) => new Action<ShipSegmentAssetModel>(asset => gameStateStore.Dispatch(new MoveShipAction(
                        activePlayer,
                        asset.Index,
                        asset.Segment,
                        position!.Value)))),
                canExecute: Position
                    .Select(position => position.HasValue));

            RotateCommand = ReactiveCommand.Create(
                segmentModel
                    .Select(segmentModel => new Action(() => gameStateStore.Dispatch(new RotateShipAction(
                        player:             segmentModel.activePlayer,
                        shipIndex:          shipIndex,
                        shipSegment:        segment,
                        targetOrientation:  segmentModel.state.Orientation switch
                        {
                            System.Drawing.Orientation.Rotate0      => System.Drawing.Orientation.Rotate270,
                            System.Drawing.Orientation.Rotate270    => System.Drawing.Orientation.Rotate180,
                            System.Drawing.Orientation.Rotate180    => System.Drawing.Orientation.Rotate90,
                            _                                       => System.Drawing.Orientation.Rotate0
                        })))));
        }

        public IObservable<ShipSegmentAssetModel> Asset { get; }

        public IObservable<bool> IsValid { get; }

        public IObservable<Orientation> Orientation { get; }

        public IObservable<Point?> Position { get; }

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegmentCommand { get; }
        
        public ICommand<Unit> RotateCommand { get; }
    }
}
