using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.State.Actions;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Setup
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
                .ToReactiveProperty();

            var boardDefinition = gameDefinition
                .Select(gameDefinition => gameDefinition.GameBoard)
                .ToReactiveProperty();

            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .Where(activePlayer => activePlayer.HasValue)
                .Select(activePlayer => activePlayer!.Value)
                .ToReactiveProperty();

            BoardPositions = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                    .ThenBy(position => position.X)
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ToReactiveProperty();

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ToReactiveProperty();

            ShipSegments = gameDefinition
                .Select(definition => definition.Ships
                    .SelectMany((ship, shipIndex) => ship.Segments
                        .Select(segment => shipSegmentFactory.Create(segment, shipIndex)))
                    .ToImmutableArray())
                .ToReactiveProperty();

            CompleteSetupCommand = ReactiveCommand.Create(
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

            RandomizeShipsCommand = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() => gameStateStore.Dispatch(new RandomizeShipsAction(
                        activePlayer,
                        random)))));
        }

        public IReadOnlyObservableProperty<ImmutableArray<SetupGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }

        public ICommand<Unit> CompleteSetupCommand { get; }

        public ICommand<Unit> RandomizeShipsCommand { get; }

        public IReadOnlyObservableProperty<ImmutableArray<SetupGamespaceShipSegmentViewModel>> ShipSegments { get; }
    }
}
