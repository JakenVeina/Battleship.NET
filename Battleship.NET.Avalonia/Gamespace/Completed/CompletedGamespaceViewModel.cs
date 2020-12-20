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

namespace Battleship.NET.Avalonia.Gamespace.Completed
{
    public class CompletedGamespaceViewModel
    {
        public CompletedGamespaceViewModel(
            CompletedGamespaceBoardTileViewModelFactory boardTileFactory,
            IStore<GameStateModel> gameStateStore,
            Random random,
            IStore<ViewStateModel> viewStateStore)
        {
            var boardDefinition = gameStateStore
                .Select(gameState => gameState.Definition.GameBoard)
                .ShareReplayDistinct(1);

            BoardSize = boardDefinition
                .Select(definition => definition.Size)
                .ShareReplayDistinct(1);

            BoardTiles = boardDefinition
                .Select(definition => definition.Positions
                    .OrderBy(position => position.Y)
                        .ThenBy(position => position.X)
                    .Select(position => boardTileFactory.Create(position))
                    .ToImmutableArray())
                .ShareReplayDistinct(1);

            BeginSetup = ReactiveCommand.Create(() =>
            {
                gameStateStore.Dispatch(new BeginSetupAction());
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player1, random));
                gameStateStore.Dispatch(new RandomizeShipsAction(GamePlayer.Player2, random));
                viewStateStore.Dispatch(new SetActivePlayerAction(GamePlayer.Player1));
            });

            ToggleActivePlayer = ReactiveCommand.Create(() => viewStateStore.Dispatch(new ToggleActivePlayerAction()));
        }

        public IObservable<Size> BoardSize { get; }

        public IObservable<ImmutableArray<CompletedGamespaceBoardTileViewModel>> BoardTiles { get; }

        public ICommand<Unit> BeginSetup { get; }

        public ICommand<Unit> ToggleActivePlayer { get; }
    }
}
