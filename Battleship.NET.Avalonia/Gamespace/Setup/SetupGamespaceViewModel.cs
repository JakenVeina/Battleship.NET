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
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .DistinctUntilChanged();

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .DistinctUntilChanged();

            BoardTiles = boardDefinition
                .Select(definition => definition.Positions
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
                    (isPlayer1Valid, isPlayer2Valid) => isPlayer1Valid && isPlayer2Valid)
                .DistinctUntilChanged();
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<SetupGamespaceBoardTileViewModel>> BoardTiles { get; }

        public IObservable<bool> IsBoardValid { get; }
    }
}
