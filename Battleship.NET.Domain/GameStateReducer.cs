using ReduxSharp;

using Battleship.NET.Domain.Actions;
using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain
{
    public class GameStateReducer
        : IReducer<GameStateModel>
    {
        public GameStateModel Invoke<TAction>(GameStateModel state, TAction action)
            => action switch
            {
                BeginSetupAction _                          => state.BeginSetup(),
                CompleteGameAction completeGameAction       => state.CompleteGame(completeGameAction.Winner),
                CompleteSetupAction completeSetupAction     => state.CompleteSetup(completeSetupAction.Player),
                EndTurnAction _                             => state.EndTurn(),
                FireShotAction fireShotAction               => state.FireShot(fireShotAction.Position),
                MoveShipAction moveShipAction               => state.MoveShip(moveShipAction.Player, moveShipAction.ShipIndex, moveShipAction.ShipSegment, moveShipAction.TargetPosition),
                RandomizeShipsAction randomizeShipsAction   => state.RandomizeShips(randomizeShipsAction.Player, randomizeShipsAction.Random),
                RotateShipAction rotateShipAction           => state.RotateShip(rotateShipAction.Player, rotateShipAction.ShipIndex, rotateShipAction.ShipSegment, rotateShipAction.TargetOrientation),
                StartGameAction startGameAction             => state.StartGame(startGameAction.FirstPlayer),
                TogglePauseAction _                         => state.TogglePause(),
                UpdateRuntimeAction updateRuntimeAction     => state.UpdateRuntime(updateRuntimeAction.Now),
                _                                           => state
            };
    }
}
