using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.State.Models
{
    public class ViewStateModel
    {
        public static readonly ViewStateModel Default
            = new ViewStateModel(default);

        public ViewStateModel(
            GamePlayer? activePlayer)
        {
            ActivePlayer = activePlayer;
        }

        public GamePlayer? ActivePlayer { get; }


        public ViewStateModel SetActivePlayer(GamePlayer? activePlayer)
            => new ViewStateModel(activePlayer);

        public ViewStateModel ToggleActivePlayer()
            => new ViewStateModel(ActivePlayer switch
            {
                GamePlayer.Player1  => GamePlayer.Player2,
                GamePlayer.Player2  => GamePlayer.Player1,
                _                   => null
            });
    }
}
