using System.Reactive.Linq;
using System.Windows;

using Battleship.NET.Domain.Models;
using Battleship.NET.WPF.Player;

namespace Battleship.NET.WPF.Game
{
    public class GameViewModel
    {
        public GameViewModel(
            PlayerViewModelFactory playerViewModelFactory)
        {
            // TODO: Implement this
            Gamespace = Observable.Never<object>()
                .ToReactiveProperty();

            Player1 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player1);
            Player2 = playerViewModelFactory.CreatePlayerViewModel(GamePlayer.Player2);
        }

        public IReadOnlyObservableProperty<object> Gamespace { get; }

        public PlayerViewModel Player1 { get; }

        public PlayerViewModel Player2 { get; }
    }
}
