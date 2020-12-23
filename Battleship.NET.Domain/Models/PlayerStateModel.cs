using System;
using System.Collections.Generic;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class PlayerStateModel
    {
        public static PlayerStateModel CreateIdle(
                IEnumerable<ShipDefinitionModel> ships)
            => new PlayerStateModel(
                false,
                GameBoardStateModel.CreateIdle(ships),
                false,
                TimeSpan.Zero,
                0);

        public PlayerStateModel(
            bool canFireShot,
            GameBoardStateModel gameBoard,
            bool isSetupComplete,
            TimeSpan playTime,
            int wins)
        {
            CanFireShot = canFireShot;
            GameBoard = gameBoard;
            IsSetupComplete = isSetupComplete;
            PlayTime = playTime;
            Wins = wins;
        }


        public bool CanFireShot { get; }

        public GameBoardStateModel GameBoard { get; }

        public bool IsSetupComplete { get; }

        public TimeSpan PlayTime { get; }

        public int Wins { get; }


        public bool CanCompleteSetup(
                GameBoardDefinitionModel gameBoard,
                IEnumerable<ShipDefinitionModel> ships)
            => !IsSetupComplete
                && GameBoard.IsValid(gameBoard, ships);

        public bool CanReceiveShot(
                Point position)
            => GameBoard.CanReceiveShot(position);


        public PlayerStateModel BeginSetup()
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.ClearShots(),
                false,
                PlayTime,
                Wins);

        public PlayerStateModel CompleteSetup()
            => new PlayerStateModel(
                CanFireShot,
                GameBoard,
                true,
                PlayTime,
                Wins);

        public PlayerStateModel FireShot(bool isHit)
            => new PlayerStateModel(
                isHit,
                GameBoard,
                IsSetupComplete,
                PlayTime,
                Wins);

        public PlayerStateModel IncrementPlayTime(
                TimeSpan increment)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard,
                IsSetupComplete,
                PlayTime + increment,
                Wins);

        public PlayerStateModel IncrementWins()
            => new PlayerStateModel(
                CanFireShot,
                GameBoard,
                IsSetupComplete,
                PlayTime,
                Wins + 1);

        public PlayerStateModel MoveShip(
                int shipIndex,
                Point shipSegment,
                Point targetPosition)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.MoveShip(shipIndex, shipSegment, targetPosition),
                IsSetupComplete,
                PlayTime,
                Wins);

        public PlayerStateModel RandomizeShips(
                GameBoardDefinitionModel gameBoardDefinition,
                IReadOnlyCollection<ShipDefinitionModel> shipDefinitions,
                Random random)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.RandomzieShips(gameBoardDefinition, shipDefinitions, random),
                IsSetupComplete,
                PlayTime,
                Wins);

        public PlayerStateModel ReceiveShot(
                Point position,
                IEnumerable<ShipDefinitionModel> ships)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.ReceiveShot(position, ships),
                IsSetupComplete,
                PlayTime,
                Wins);

        public PlayerStateModel RotateShip(
                int shipIndex,
                Point shipSegment,
                Orientation targetOrientation)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.RotateShip(shipIndex, shipSegment, targetOrientation),
                IsSetupComplete,
                PlayTime,
                Wins);

        public PlayerStateModel StartTurn()
            => new PlayerStateModel(
                true,
                GameBoard,
                IsSetupComplete,
                PlayTime,
                Wins);
    }
}
