using Redux;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Domain
{
    public class Game
        : Store<GameStateModel>
    {
        public Game(
                GameStateModel initialState)
            : base(ReduceState, initialState)
        { }

        public static GameStateModel ReduceState(
                GameStateModel previousState,
                IAction action)
            => action switch
            {
                BeginSetupAction _                      => previousState.BeginSetup(),
                CompleteSetupAction completeSetupAction => previousState.CompleteSetup(completeSetupAction.Player),
                EndTurnAction _                         => previousState.EndTurn(),
                FireShotAction fireShotAction           => previousState.FireShot(fireShotAction.Position),
                MoveShipAction moveShipAction           => previousState.MoveShip(moveShipAction.Player, moveShipAction.ShipIndex, moveShipAction.ShipSegment, moveShipAction.TargetPosition),
                RotateShipAction rotateShipAction       => previousState.RotateShip(rotateShipAction.Player, rotateShipAction.ShipIndex, rotateShipAction.ShipSegment, rotateShipAction.TargetOrientation),
                StartGameAction startGameAction         => previousState.StartGame(startGameAction.FirstPlayer),
                TogglePauseAction _                     => previousState.TogglePause(),
                UpdateRuntimeAction updateRuntimeAction => previousState.UpdateRuntime(updateRuntimeAction.Now),
                _                                       => previousState
            };
    }
}
