using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class StartGameAction
        : IAction
    {
        public StartGameAction(
            GamePlayer firstPlayer)
        {
            FirstPlayer = firstPlayer;
        }

        public GamePlayer FirstPlayer { get; }
    }
}
