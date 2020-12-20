using ReduxSharp;

using Battleship.NET.Avalonia.State.Actions;
using Battleship.NET.Avalonia.State.Models;

namespace Battleship.NET.Avalonia.State
{
    public class ViewStateReducer
        : IReducer<ViewStateModel>
    {
        public ViewStateModel Invoke<TAction>(ViewStateModel state, TAction action)
            => action switch
            {
                SetActivePlayerAction setActivePlayerAction => state.SetActivePlayer(setActivePlayerAction.Player),
                ToggleActivePlayerAction _                  => state.ToggleActivePlayer(),
                _                                           => state
            };
    }
}
