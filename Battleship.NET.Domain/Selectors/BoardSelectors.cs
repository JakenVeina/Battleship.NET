using System;
using System.Drawing;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Selectors
{
    public static class BoardSelectors
    {
        public static readonly SelectorSet<GameStateModel, bool, GamePlayer> IsValid
            = SelectorSet.Create<GameStateModel, bool, GamePlayer>(
                // TODO: Implement this
                player => throw new NotImplementedException());

        // TODO: Implement this
        public static readonly Func<GameStateModel, Size> Size
            = _ => throw new NotImplementedException();
    }
}
