using ReduxSharp;

using Battleship.NET.WPF.State.Actions;
using Battleship.NET.WPF.State.Models;

namespace Battleship.NET.WPF.State
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
