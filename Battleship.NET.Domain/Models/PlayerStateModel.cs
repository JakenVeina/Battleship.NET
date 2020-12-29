using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class PlayerStateModel
    {
        public static PlayerStateModel CreateIdle(
                IEnumerable<ShipDefinitionModel> ships)
            => new PlayerStateModel(
                canFireShot:        false,
                gameBoard:          GameBoardStateModel.CreateIdle(ships),
                isSetupComplete:    false,
                playTime:           TimeSpan.Zero,
                wins:               0);

        private PlayerStateModel(
            bool canFireShot,
            GameBoardStateModel gameBoard,
            bool isSetupComplete,
            TimeSpan playTime,
            int wins)
        {
            CanFireShot     = canFireShot;
            GameBoard       = gameBoard;
            IsSetupComplete = isSetupComplete;
            PlayTime        = playTime;
            Wins            = wins;
        }

        private PlayerStateModel(PlayerStateModel original)
        {
            CanFireShot     = original.CanFireShot;
            GameBoard       = original.GameBoard;
            IsSetupComplete = original.IsSetupComplete;
            PlayTime        = original.PlayTime;
            Wins            = original.Wins;
        }


        public bool CanFireShot { get; private init; }

        public GameBoardStateModel GameBoard { get; private init; }

        public bool IsSetupComplete { get; private init; }

        public TimeSpan PlayTime { get; private init; }

        public int Wins { get; private init; }


        public PlayerStateModel BeginSetup()
            => new(this)
            {
                GameBoard       = GameBoard.ClearShots(),
                IsSetupComplete = false
            };

        public PlayerStateModel CompleteSetup()
            => new(this)
            {
                IsSetupComplete = true,
            };

        public PlayerStateModel FireShot(ShotOutcome outcome)
            => new(this)
            {
                CanFireShot = (outcome == ShotOutcome.Hit)
            };

        public PlayerStateModel IncrementWins()
            => new(this)
            {
                Wins = Wins + 1
            };

        public PlayerStateModel MoveShip(
                int shipIndex,
                Point shipSegment,
                Point targetPosition)
            => new(this)
            {
                GameBoard = GameBoard.MoveShip(shipIndex, shipSegment, targetPosition),
            };

        public PlayerStateModel PlaceShips(
                ImmutableList<ShipStateModel> ships)
            => new(this)
            {
                GameBoard = GameBoard.PlaceShips(ships)
            };

        public PlayerStateModel ReceiveShot(
                Point position,
                ShotOutcome outcome)
            => new(this)
            {
                GameBoard = GameBoard.ReceiveShot(position, outcome),
            };

        public PlayerStateModel RotateShip(
                int shipIndex,
                Point shipSegment,
                Orientation targetOrientation)
            => new(this)
            {
                GameBoard = GameBoard.RotateShip(shipIndex, shipSegment, targetOrientation),
            };

        public PlayerStateModel StartTurn()
            => new(this)
            {
                CanFireShot = true
            };

        public PlayerStateModel IncrementPlayTime(
                TimeSpan increment)
            => new(this)
            {
                PlayTime = PlayTime + increment,
            };
    }
}
