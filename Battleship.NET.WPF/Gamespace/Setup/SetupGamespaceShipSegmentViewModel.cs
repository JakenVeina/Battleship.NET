using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentViewModel
    {
        public SetupGamespaceShipSegmentViewModel(
            IStore<GameStateModel> gameStateStore,
            System.Drawing.Point segment,
            int shipIndex,
            IStore<ViewStateModel> viewStateStore)
        {
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
                .ToReactiveProperty();

            var segmentModel = model
                .Select(model => 
                (
                    activePlayer:       model.activePlayer,
                    boardPositions:     model.boardPositions,
                    definition:         model.shipDefinitions[shipIndex],
                    state:              model.boardState.Ships[shipIndex]
                ))
                .ToReactiveProperty();

            Asset = segmentModel
                .Select(segmentModel=> new ShipSegmentAssetModel(
                    index:          shipIndex,
                    name:           segmentModel.definition.Name,
                    orientation:    segmentModel.state.Orientation,
                    segment:        segment))
                .ToReactiveProperty();

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
                .ToReactiveProperty();

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
                .ToReactiveProperty();

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
                            Orientation.Rotate0     => Orientation.Rotate270,
                            Orientation.Rotate270   => Orientation.Rotate180,
                            Orientation.Rotate180   => Orientation.Rotate90,
                            _                       => Orientation.Rotate0
                        })))));
        }

        public IReadOnlyObservableProperty<ShipSegmentAssetModel> Asset { get; }

        public IReadOnlyObservableProperty<bool> IsValid { get; }

        public IReadOnlyObservableProperty<System.Drawing.Point?> Position { get; }

        public ICommand<ShipSegmentAssetModel> ReceiveShipSegmentCommand { get; }
        
        public ICommand<Unit> RotateCommand { get; }
    }
}
