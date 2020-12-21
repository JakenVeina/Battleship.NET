using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.Ship;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class RunningGamespaceBoardPositionViewModel
    {
        public RunningGamespaceBoardPositionViewModel(
            IStore<GameStateModel> gameStateStore,
            Point position)
        {
            FireShotCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new FireShotAction(position)),
                canExecute: gameStateStore
                    .Select(gameState => gameState.CanFireShot(position))
                    .DistinctUntilChanged());

            Position = position;

            ShipAsset = gameStateStore
                .Select(gameState =>
                (
                    hits:               gameState.OpponentPlayerState.GameBoard.Hits,
                    shipDefinitions:    gameState.Definition.Ships,
                    shipStates:         gameState.OpponentPlayerState.GameBoard.Ships
                ))
                .DistinctUntilChanged()
                .Select(model => Enumerable.Zip(
                        model.shipDefinitions,
                        model.shipStates,
                        (definition, state) => 
                        (
                            definition:     definition,
                            segmentModels:  Enumerable.Zip(
                                definition.Segments,
                                state.EnumerateSegmentPositions(definition),
                                (segment, position) => (segment, position)),
                            state:          state
                        ))
                    .Where(shipModel => shipModel.segmentModels.All(segmentModel => model.hits.Contains(segmentModel.position)))
                    .SelectMany((shipModel, index) => shipModel.segmentModels
                        .Select(segmentModel => 
                        (
                            index:          index,
                            name:           shipModel.definition.Name,
                            segment:        segmentModel.segment,
                            orientation:    shipModel.state.Orientation,
                            position:       segmentModel.position
                        )))
                    .Where(segmentModel => segmentModel.position == position)
                    .Select(segmentModel => new ShipSegmentAssetModel(
                        index:          segmentModel.index,
                        name:           segmentModel.name,
                        orientation:    segmentModel.orientation,
                        segment:        segmentModel.segment))
                    .FirstOrDefault())
                .ShareReplayDistinct(1);

            ShotAsset = gameStateStore
                .Select(gameState => 
                (
                    hits:   gameState.OpponentPlayerState.GameBoard.Hits,
                    misses: gameState.OpponentPlayerState.GameBoard.Misses
                ))
                .DistinctUntilChanged()
                .Select(shots => shots switch
                {
                    _ when shots.hits.Contains(position)    => ShotAssetModel.Hit,
                    _ when shots.misses.Contains(position)  => ShotAssetModel.Miss,
                    _                                       => null
                })
                .ShareReplayDistinct(1);
        }

        public ICommand<Unit> FireShotCommand { get; }

        public Point Position { get; }

        public IObservable<ShipSegmentAssetModel?> ShipAsset { get; }

        public IObservable<ShotAssetModel?> ShotAsset { get; }
    }
}
