using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.State.Actions
{
    public class SetActivePlayerAction
    {
        public SetActivePlayerAction(
            GamePlayer? player)
        {
            Player = player;
        }

        public GamePlayer? Player { get; }
    }
}
