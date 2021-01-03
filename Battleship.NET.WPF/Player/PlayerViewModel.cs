using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Windows;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Ship;

namespace Battleship.NET.WPF.Player
{
    public class PlayerViewModel
    {
        public PlayerViewModel(
            GamePlayer player)
        {
            // TODO: Implement this
            IsActive = Observable.Never<bool>()
                .ToReactiveProperty();

            // TODO: Implement this
            Name = Observable.Never<string>()
                .ToReactiveProperty();

            // TODO: Implement this
            PlayTime = Observable.Never<TimeSpan>()
                .ToReactiveProperty();

            // TODO: Implement this
            ShipStatuses = Observable.Never<ImmutableArray<ShipStatusViewModel>>()
                .ToReactiveProperty();

            // TODO: Implement this
            Wins = Observable.Never<int>()
                .ToReactiveProperty();
        }

        public IReadOnlyObservableProperty<bool> IsActive { get; }

        public IReadOnlyObservableProperty<string> Name { get; }

        public IReadOnlyObservableProperty<TimeSpan> PlayTime { get; }

        public IReadOnlyObservableProperty<ImmutableArray<ShipStatusViewModel>> ShipStatuses { get; }

        public IReadOnlyObservableProperty<int> Wins { get; }
    }
}
