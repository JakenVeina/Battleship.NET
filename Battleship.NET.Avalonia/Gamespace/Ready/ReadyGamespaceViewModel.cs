using System;
using System.Reactive;
using System.Windows.Input;

using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Avalonia.Gamespace.Ready
{
    public class ReadyGamespaceViewModel
    {
        public ReadyGamespaceViewModel(
            IStore<GameStateModel> gameStateStore,
            Random random)
        {
            StartGame = ReactiveCommand.Create(
                () => gameStateStore.Dispatch(new StartGameAction(
                        firstPlayer: EnumEx.GetValues<GamePlayer>()
                            .PickRandom(random))));
        }

        public ICommand<Unit> StartGame { get; }
    }
}
