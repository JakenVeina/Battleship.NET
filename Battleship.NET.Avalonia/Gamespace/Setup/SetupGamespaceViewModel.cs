using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Avalonia.State.Actions;
using Battleship.NET.Avalonia.State.Models;
using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceViewModel
    {
        public SetupGamespaceViewModel(
            SetupGamespaceBoardPositionViewModelFactory boardPositionFactory,
            IStore<GameStateModel> gameStateStore,
            Random random,
            SetupGamespaceShipSegmentViewModelFactory shipSegmentFactory,
            IStore<ViewStateModel> viewStateStore)
        {
            var gameDefinition = gameStateStore
                .Select(gameState => gameState.Definition)
                .ShareReplayDistinct(1);

            var boardDefinition = gameDefinition
                .Select(gameDefinition => gameDefinition.GameBoard)
                .ShareReplayDistinct(1);

            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .Where(activePlayer => activePlayer.HasValue)
                .Select(activePlayer => activePlayer!.Value)
                .ShareReplayDistinct(1);

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ShareReplayDistinct(1);

            BoardPositions = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                    .ThenBy(position => position.X)
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);

            ShipSegments = gameDefinition
                .Select(definition => definition.Ships
                    .SelectMany((ship, shipIndex) => ship.Segments
                        .Select(segment => shipSegmentFactory.Create(segment, shipIndex)))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);

            CompleteSetup = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() =>
                    {
                        gameStateStore.Dispatch(new CompleteSetupAction(activePlayer));
                        viewStateStore.Dispatch(new ToggleActivePlayerAction());
                    })),
                Observable.CombineLatest(
                        gameStateStore,
                        activePlayer,
                        (gameState, activePlayer) => gameState.CanCompleteSetup(activePlayer))
                    .DistinctUntilChanged());

            RandomizeShips = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() => gameStateStore.Dispatch(new RandomizeShipsAction(
                        activePlayer,
                        random)))));
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IObservable<ImmutableArray<SetupGamespaceShipSegmentViewModel>> ShipSegments { get; }

        public ICommand<Unit> CompleteSetup { get; }
     
        public ICommand<Unit> RandomizeShips { get; }
    }
}
