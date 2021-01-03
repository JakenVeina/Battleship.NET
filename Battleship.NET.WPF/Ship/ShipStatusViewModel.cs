using System.Reactive.Linq;
using System.Windows;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Ship
{
    public class ShipStatusViewModel
    {
        public ShipStatusViewModel(
            GamePlayer player,
            int shipIndex)
        {
            // TODO: Implement this
            IsSunk = Observable.Never<bool>()
                .ToReactiveProperty();

            // TODO: Implement this
            Name = Observable.Never<string>()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsSunk { get; }

        public IReadOnlyObservableProperty<string> Name { get; }
    }
}
