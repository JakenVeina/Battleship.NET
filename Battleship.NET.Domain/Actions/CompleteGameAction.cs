using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class CompleteGameAction
        : IAction
    {
        public CompleteGameAction(GamePlayer winner)
        {
            Winner = winner;
        }

        public GamePlayer Winner { get; }
    }
}
