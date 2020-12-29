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
using Battleship.NET.Domain.Selectors;
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
            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .WhereNotNull()
                .DistinctUntilChanged()
                .ShareReplay(1);

            BoardPositions = gameStateStore
                .Select(gameState => gameState.Definition)
                .DistinctUntilChanged()
                .Select(definition => definition.GameBoard.Positions
                    .Select(position => boardPositionFactory.Create(position))
                    .ToImmutableArray())
                .ToReactiveProperty();

            BoardSize = gameStateStore
                .Select(BoardSelectors.Size)
                .ToReactiveProperty();

            CompleteSetupCommand = ReactiveCommand.Create(
                execute: activePlayer
                    .Select(activePlayer => new Action(() =>
                    {
                        gameStateStore.Dispatch(new CompleteSetupAction(activePlayer));
                        viewStateStore.Dispatch(new ToggleActivePlayerAction());
                    })),
                canExecute: activePlayer
                    .Select(activePlayer => gameStateStore
                        .Select(BoardSelectors.IsValid[activePlayer]))
                    .Switch()
                    .DistinctUntilChanged());

            RandomizeShipsCommand = ReactiveCommand.Create(
                activePlayer
                    .Select(activePlayer => new Action(() => gameStateStore.Dispatch(new RandomizeShipsAction(
                        activePlayer,
                        random)))));

            ShipSegments = gameStateStore
                .Select(gameState => gameState.Definition.Ships)
                .DistinctUntilChanged()
                .Select(definitions => definitions
                    .SelectMany((ship, shipIndex) => ship.Segments
                        .Select(segment => shipSegmentFactory.Create(segment, shipIndex)))
                    .ToImmutableArray())
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<SetupGamespaceBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }

        public ICommand<Unit> CompleteSetupCommand { get; }

        public ICommand<Unit> RandomizeShipsCommand { get; }

        public IReadOnlyObservableProperty<ImmutableArray<SetupGamespaceShipSegmentViewModel>> ShipSegments { get; }
    }
}
