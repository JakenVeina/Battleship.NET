﻿using Redux;

using Battleship.NET.Domain.Models;
using Battleship.NET.Domain.Actions;

namespace Battleship.NET.Domain
{
    public class GameStateStore
        : Store<GameStateModel>
    {
        public GameStateStore(
                GameStateModel initialState,
                params Middleware<GameStateModel>[] middlewares)
            : base(Reduce, initialState, middlewares)
        { }

        private static GameStateModel Reduce(
                GameStateModel previousState,
                IAction action)
            => action switch
            {
                BeginSetupAction _                      => previousState.BeginSetup(),
                CompleteSetupAction completeSetupAction => previousState.CompleteSetup(completeSetupAction.Player),
                EndTurnAction _                         => previousState.EndTurn(),
                FireShotAction fireShotAction           => previousState.FireShot(fireShotAction.Position),
                MoveShipAction moveShipAction           => previousState.MoveShip(moveShipAction.Player, moveShipAction.ShipIndex, moveShipAction.Orientation, moveShipAction.Position),
                StartGameAction startGameAction         => previousState.StartGame(startGameAction.FirstPlayer),
                TogglePauseAction _                     => previousState.TogglePause(),
                UpdateRuntimeAction updateRuntimeAction => previousState.UpdateRuntime(updateRuntimeAction.Now),
                _                                       => previousState
            };
    }
}
