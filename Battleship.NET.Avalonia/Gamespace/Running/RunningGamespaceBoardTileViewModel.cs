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
            Content = gameStateStore
                .Select(gameState =>
                (
                    gameBoard:          gameState.CurrentPlayerState.GameBoard,
                    shipDefinitions:    gameState.Definition.Ships
                ))
                .DistinctUntilChanged()
                .Select(model =>
                {
                    return model.gameBoard.Misses.Contains(position)    ? new ShotAssetModel(false)
                        : !model.gameBoard.Hits.Contains(position)      ? null
                                                                        : 


                    if (model.gameBoard.Misses.Contains(position))
                        return new ShotAssetModel(false);

                    if (!model.gameBoard.Hits.Contains(position))
                        return null;

                    var shipSegmentModel = Enumerable.Zip(
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
                                segment:        segment,
                                shipDefinition: shipModel.definition,
                                shipState:      shipModel.state
                            )))
                        .First(shipSegmentModel => shipSegmentModel.position == position);

                    return shipSegmentModel.shipState.EnumerateSegmentPositions(shipSegmentModel.shipDefinition)
                                .All(segmentPosition => model.gameBoard.Hits.Contains(segmentPosition))
                            ? new ShipSegmentAssetModel(
                                shipSegmentModel.segment,
                                shipSegmentModel.shipDefinition.Name)
                            : new ShotAssetModel(true)
                        as object;
                })
                .DistinctUntilChanged();

            FireShot = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new FireShotAction(position)),
                gameStateStore
                    .Select(gameState => gameState.CanFireShot(position))
                    .DistinctUntilChanged());
        }

        public IObservable<object?> Content { get; }

        public ICommand<Unit> FireShot { get; }
    }
}
