using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionViewModel
        : GameBoardPositionViewModelBase
    {
        public CompletedGamespaceBoardPositionViewModel(
                IStore<GameStateModel> gameStateStore,
                System.Drawing.Point position,
                IStore<ViewStateModel> viewStateStore)
            : base(position)
        {
            var activePlayer = viewStateStore
                .Select(viewState => viewState.ActivePlayer)
                .WhereNotNull()
                .DistinctUntilChanged()
                .ShareReplay(1);

            var shipSegmentPlacement = activePlayer
                .Select(activePlayer => gameStateStore
                    .Select(BoardSelectors.ShipSegmentPlacement[(activePlayer, position)]))
                .Switch()
                .DistinctUntilChanged()
                .ShareReplay(1);

            IsShipSunk = shipSegmentPlacement
                .WithLatestFrom(
                    activePlayer,
                    (shipSegmentPlacement, activePlayer) => (shipSegmentPlacement is not null)
                        ? gameStateStore
                            .Select(ShipSelectors.IsSunk[(activePlayer, shipSegmentPlacement.ShipIndex)])
                        : Observable.Return(false))
                .Switch()
                .ToReactiveProperty();

            ShipAsset = Observable.CombineLatest(
                    gameStateStore
                        .Select(gameState => gameState.Definition),
                    shipSegmentPlacement,
                    (definition, shipSegmentPlacement) => (shipSegmentPlacement is null)
                        ? null
                        : new ShipSegmentAssetModel(
                            orientation:    shipSegmentPlacement.Orientation,
                            segment:        shipSegmentPlacement.Segment,
                            shipIndex:      shipSegmentPlacement.ShipIndex,
                            shipName:       definition.Ships[shipSegmentPlacement.ShipIndex].Name))
                .ToReactiveProperty();

            ShotOutcome = activePlayer
                .Select(activePlayer => gameStateStore
                    .Select(BoardSelectors.ShotOutcome[(activePlayer, position)]))
                .Switch()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsShipSunk { get; }
        
        public IReadOnlyObservableProperty<ShipSegmentAssetModel?> ShipAsset { get; }

        public IReadOnlyObservableProperty<ShotOutcome?> ShotOutcome { get; }
    }
}
