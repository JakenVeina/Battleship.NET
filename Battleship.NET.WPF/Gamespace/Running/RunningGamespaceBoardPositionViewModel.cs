using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceBoardPositionViewModel
    {
        public RunningGamespaceBoardPositionViewModel(
            IStore<GameStateModel> gameStateStore,
            System.Drawing.Point position)
        {
            var opponentPlayer = gameStateStore
                .Select(gameState => (gameState.CurrentPlayer == GamePlayer.Player1)
                    ? GamePlayer.Player2
                    : GamePlayer.Player1)
                .DistinctUntilChanged()
                .ShareReplay(1);

            FireShotCommand = ReactiveCommand.Create(
                execute:    () => gameStateStore.Dispatch(new FireShotAction(position)),
                canExecute: opponentPlayer
                    .Select(opponentPlayer => gameStateStore
                        .Select(BoardSelectors.CanReceiveShot[(opponentPlayer, position)]))
                    .Switch()
                    .DistinctUntilChanged());

            Position = position;

            ShipAsset = opponentPlayer
                .Select(opponentPlayer => gameStateStore
                    .Select(BoardSelectors.ShipSegmentPlacement[(opponentPlayer, position)]))
                .Switch()
                .WithLatestFrom(
                    opponentPlayer,
                    (shipSegmentPlacement, opponentPlayer) => (shipSegmentPlacement is not null)
                        ? Observable.CombineLatest(
                            gameStateStore
                                .Select(gameState => gameState.Definition),
                            gameStateStore
                                .Select(ShipSelectors.IsSunk[(opponentPlayer, shipSegmentPlacement.ShipIndex)]),
                            (definition, isSunk) => isSunk
                                ? new ShipSegmentAssetModel(
                                    orientation:    shipSegmentPlacement.Orientation,
                                    segment:        shipSegmentPlacement.Segment,
                                    shipIndex:      shipSegmentPlacement.ShipIndex,
                                    shipName:       definition.Ships[shipSegmentPlacement.ShipIndex].Name)
                                : null)
                        : Observable.Return<ShipSegmentAssetModel?>(null))
                .Switch()
                .ToReactiveProperty();

            ShotOutcome = opponentPlayer
                .Select(opponentPlayer => gameStateStore
                    .Select(BoardSelectors.ShotOutcome[(opponentPlayer, position)]))
                .Switch()
                .ToReactiveProperty();
        }

        public ICommand<Unit> FireShotCommand { get; }

        public System.Drawing.Point Position { get; }

        public IReadOnlyObservableProperty<ShipSegmentAssetModel?> ShipAsset { get; }

        public IReadOnlyObservableProperty<ShotOutcome?> ShotOutcome { get; }
    }
}
