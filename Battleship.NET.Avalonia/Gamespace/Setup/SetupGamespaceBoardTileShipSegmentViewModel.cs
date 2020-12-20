using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using Redux;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceBoardTileShipSegmentViewModel
    {
        public SetupGamespaceBoardTileShipSegmentViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position,
            int shipIndex,
            IStore<ViewStateModel> viewStateStore)
        {
            var segmentModel = Observable.CombineLatest(
                    gameStateStore,
                    viewStateStore,
                    (gameState, viewState) => 
                    (
                        activePlayer:   viewState.ActivePlayer,
                        definition:     gameState.Definition.Ships[shipIndex],
                        player1State:   gameState.Player1.GameBoard.Ships[shipIndex],
                        player2State:   gameState.Player2.GameBoard.Ships[shipIndex]
                    ))
                .Where(model => model.activePlayer.HasValue)
                .Select(model => 
                (
                    activePlayer:   model.activePlayer!.Value,
                    definition:     model.definition,
                    state:          (model.activePlayer!.Value == GamePlayer.Player1)
                                        ? model.player1State
                                        : model.player2State
                ))
                .DistinctUntilChanged()
                .Select(model => model.definition.Segments
                    .Select(segment => 
                    (
                        activePlayer:   model.activePlayer,
                        name:           model.definition.Name,
                        orientation:    model.state.Orientation,
                        position:       segment
                                            .RotateOrigin(model.state.Orientation)
                                            .Translate(model.state.Position),
                        segment:        segment
                    ).ToNullable())
                    .FirstOrDefault(model => model!.Value.position == position))
                .ShareReplayDistinct(1);

            Asset = segmentModel
                .Select(model => model.HasValue
                    ? new ShipSegmentAssetModel(
                        model.Value.segment,
                        model.Value.name)
                    : null)
                .ShareReplayDistinct(1);

            HasShip = segmentModel
                .Select(model => model.HasValue)
                .ShareReplayDistinct(1);

            IsShipValid = Observable.CombineLatest(
                    gameStateStore,
                    viewStateStore,
                    (gameState, viewState) => 
                    (
                        activePlayer:       viewState.ActivePlayer,
                        shipDefinitions:    gameState.Definition.Ships,
                        player1ShipStates:  gameState.Player1.GameBoard.Ships,
                        player2ShipStates:  gameState.Player2.GameBoard.Ships
                    ))
                .Where(model => model.activePlayer.HasValue)
                .Select(model => 
                (
                    shipDefinitions:    model.shipDefinitions,
                    shipStates:         (model.activePlayer == GamePlayer.Player1)
                                            ? model.player1ShipStates
                                            : model.player2ShipStates
                ))
                .DistinctUntilChanged()
                .Select(model => !Enumerable.Zip(
                        model.shipDefinitions,
                        model.shipStates,
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
                .ShareReplayDistinct(1);

            Movement = segmentModel
                .Select(model => model.HasValue
                    ? new ShipSegmentMovementModel(
                        model.Value.segment,
                        shipIndex)
                    : null)
                .ShareReplayDistinct(1);

            Orientation = segmentModel
                .Select(model => model?.orientation)
                .ShareReplayDistinct(1);

            Rotate = ReactiveCommand.Create(
                segmentModel
                    .Where(model => model.HasValue)
                    .Select(model => new Action(() => gameStateStore.Dispatch(new RotateShipAction(
                        model!.Value.activePlayer,
                        shipIndex,
                        model!.Value.segment,
                        model!.Value.orientation switch
                        {
                            Rotation.Rotate0    => Rotation.Rotate270,
                            Rotation.Rotate270  => Rotation.Rotate180,
                            Rotation.Rotate180  => Rotation.Rotate90,
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
