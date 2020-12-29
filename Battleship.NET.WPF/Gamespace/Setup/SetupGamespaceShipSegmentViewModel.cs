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
using Battleship.NET.Domain.Selectors;
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
            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .WhereNotNull()
                .DistinctUntilChanged()
                .ShareReplay(1);

            Asset = Observable.CombineLatest(
                    gameStateStore
                        .Select(gameState => gameState.Definition.Ships[shipIndex].Name)
                        .DistinctUntilChanged(),
                    activePlayer
                        .Select(activePlayer => (activePlayer == GamePlayer.Player1)
                            ? gameStateStore.Select(gameState => gameState.Player1.GameBoard.Ships[shipIndex].Orientation)
                            : gameStateStore.Select(gameState => gameState.Player2.GameBoard.Ships[shipIndex].Orientation))
                        .Switch()
                        .DistinctUntilChanged(),
                    (name, orientation) => new ShipSegmentAssetModel(
                        shipIndex:          shipIndex,
                        shipName:           name,
                        orientation:    orientation,
                        segment:        segment))
                .ToReactiveProperty();

            IsValid = activePlayer
                .Select(activePlayer => gameStateStore
                    .Select(ShipSelectors.IsValid[(activePlayer, shipIndex)]))
                .Switch()
                .ToReactiveProperty();

            Position = Observable.CombineLatest(
                    gameStateStore
                        .Select(BoardSelectors.Size)
                        .DistinctUntilChanged(),
                    activePlayer
                        .Select(activePlayer => gameStateStore
                            .Select(ShipSelectors.SegmentPlacement[(activePlayer, shipIndex, segment)])
                            .Select(segmentPlacement => segmentPlacement.Position))
                        .Switch(),
                    (size, position) => (new Rectangle(System.Drawing.Point.Empty, size).Contains(position))
                        ? position.ToNullable()
                        : null)
                .ToReactiveProperty();

            ReceiveShipSegmentCommand = ReactiveCommand.Create(
                execute:    Observable.CombineLatest(
                    activePlayer,
                    Position,
                    (activePlayer, position) => new Action<ShipSegmentAssetModel>(asset => gameStateStore.Dispatch(new MoveShipAction(
                        player:         activePlayer,
                        shipIndex:      asset.ShipIndex,
                        shipSegment:    asset.Segment,
                        targetPosition: position!.Value)))), // Null-check is performed within canExecute below
                canExecute: Position
                    .Select(Position => Position.HasValue)
                    .DistinctUntilChanged());

            RotateCommand = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => gameStateStore
                        .Select<GameStateModel, Orientation>((activePlayer == GamePlayer.Player1)
                            ? gameState => gameState.Player1.GameBoard.Ships[shipIndex].Orientation
                            : gameState => gameState.Player2.GameBoard.Ships[shipIndex].Orientation)
                        .Select(orientation => 
                        (
                            activePlayer:   activePlayer,
                            orientation:    orientation
                        )))
                    .Switch()
                    .Select(@params => new Action(() => gameStateStore.Dispatch(new RotateShipAction(
                        player:             @params.activePlayer,
                        shipIndex:          shipIndex,
                        shipSegment:        segment,
                        targetOrientation:  @params.orientation switch
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
