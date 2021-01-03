using ReduxSharp;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Ship
{
    public class ShipStatusViewModelFactory
    {
        public ShipStatusViewModel Create(
                GamePlayer player,
                int shipIndex)
            => new ShipStatusViewModel(
                player:     player,
                shipIndex:  shipIndex);
    }
}
