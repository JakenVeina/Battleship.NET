using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class SetFirstPlayerAction
        : IAction
    {
        public SetFirstPlayerAction(
            GamePlayer firstPlayer)
        {
            FirstPlayer = firstPlayer;
        }

        public GamePlayer FirstPlayer { get; }
    }
}
