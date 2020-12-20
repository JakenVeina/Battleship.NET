using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.State.Actions
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
