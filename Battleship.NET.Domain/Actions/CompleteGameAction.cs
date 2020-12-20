using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class CompleteGameAction
    {
        public CompleteGameAction(GamePlayer winner)
        {
            Winner = winner;
        }

        public GamePlayer Winner { get; }
    }
}
