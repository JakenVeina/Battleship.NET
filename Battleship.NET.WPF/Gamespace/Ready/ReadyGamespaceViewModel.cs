using System;
using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.WPF.Gamespace.Ready
{
    public class ReadyGamespaceViewModel
    {
        public ReadyGamespaceViewModel(
            IStore<GameStateModel> gameStateStore,
            Random random)
        {
            StartGameCommand = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new StartGameAction(
                        firstPlayer: EnumEx.GetValues<GamePlayer>()
                            .PickRandom(random))));
        }

        public ICommand<Unit> StartGameCommand { get; }
    }
}
