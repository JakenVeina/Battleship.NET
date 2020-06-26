using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class CompleteSetupAction
        : IAction
    {
        public CompleteSetupAction(
            GamePlayer player)
        {
            Player = player;
        }

        public GamePlayer Player { get; }
    }
}
