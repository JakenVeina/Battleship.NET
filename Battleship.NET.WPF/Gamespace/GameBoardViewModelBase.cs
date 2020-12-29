using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Selectors;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardViewModelBase<TBoardPositionViewModel>
    {
        protected GameBoardViewModelBase(
            Func<System.Drawing.Point, TBoardPositionViewModel> boardPositionFactory, 
            IStore<GameStateModel> gameStateStore)
        {
            BoardPositions = gameStateStore
                .Select(gameState => gameState.Definition)
                .DistinctUntilChanged()
                .Select(definition => definition.GameBoard.Positions
                    .Select(position => boardPositionFactory.Invoke(position))
                    .ToImmutableArray())
                .ToReactiveProperty();

            BoardSize = gameStateStore
                .Select(BoardSelectors.Size)
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<ImmutableArray<TBoardPositionViewModel>> BoardPositions { get; }

        public IReadOnlyObservableProperty<System.Drawing.Size> BoardSize { get; }
    }
}
