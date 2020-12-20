using Redux;

using Battleship.NET.Avalonia.State.Actions;
using Battleship.NET.Avalonia.State.Models;

namespace Battleship.NET.Avalonia.State
{
    public class ViewStateStore
        : Store<ViewStateModel>
    {
        public ViewStateStore()
            : base(ReduceState, ViewStateModel.Default)
        { }

        public static ViewStateModel ReduceState(
                ViewStateModel previousState,
                IAction action)
            => action switch
            {
                SetActivePlayerAction setActivePlayerAction => previousState.SetActivePlayer(setActivePlayerAction.Player),
                ToggleActivePlayerAction _                  => previousState.ToggleActivePlayer(),
                _                                           => previousState
            };
    }
}
