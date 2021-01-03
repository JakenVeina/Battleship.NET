using ReduxSharp;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.Player
{
    public class PlayerViewModelFactory
    {
        public PlayerViewModel CreatePlayerViewModel(
                GamePlayer player)
            => new PlayerViewModel(
                player);
    }
}
