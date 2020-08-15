using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Setup
{
    public class SetupGamespaceViewModel
    {
        public SetupGamespaceViewModel(
            SetupGamespaceBoardTileViewModelFactory gameBoardTileViewModelFactory,
            IStore<GameStateModel> gameStateStore)
        {
            var boardPositions = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard.Positions)
                .DistinctUntilChanged();

            BoardSize = boardPositions
                .Select(positions => new Size(
                    width: positions.Max(position => position.X) + 1,
                    height: positions.Max(position => position.Y) + 1))
                .DistinctUntilChanged();

            BoardTiles = boardPositions
                .Select(positions => positions
                    .OrderBy(position => position.Y)
                        .ThenBy(position => position.X)
                    .Select(position => gameBoardTileViewModelFactory.Create(position))
                    .ToImmutableArray())
                .DistinctUntilChanged();

            var isPlayer1Valid = gameStateStore
                .Select(gameState =>
                (
                    boardDefinition: gameState.Definition.GameBoard,
                    shipDefinitions: gameState.Definition.Ships,
                    boardState: gameState.Player1.GameBoard
                ))
                .DistinctUntilChanged()
                .Select(model => model.boardState.IsValid(model.boardDefinition, model.shipDefinitions))
                .DistinctUntilChanged();

            var isPlayer2Valid = gameStateStore
                .Select(gameState =>
                (
                    boardDefinition: gameState.Definition.GameBoard,
                    shipDefinitions: gameState.Definition.Ships,
                    boardState: gameState.Player2.GameBoard
                ))
                .DistinctUntilChanged()
                .Select(model => model.boardState.IsValid(model.boardDefinition, model.shipDefinitions))
                .DistinctUntilChanged();

            IsBoardValid = Observable.CombineLatest(
                isPlayer1Valid,
                isPlayer2Valid,
                (isPlayer1Valid, isPlayer2Valid) => isPlayer1Valid && isPlayer2Valid);
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileViewModel>> BoardTiles { get; }

        public IObservable<bool> IsBoardValid { get; }
    }
}
