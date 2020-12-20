using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class CompleteSetupAction
    {
        public CompleteSetupAction(
            GamePlayer player)
        {
            Player = player;
        }

        public GamePlayer Player { get; }
    }
}
