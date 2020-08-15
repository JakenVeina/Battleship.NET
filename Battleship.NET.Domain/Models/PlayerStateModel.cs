using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

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
                0);

        public PlayerStateModel(
            bool canFireShot,
            GameBoardStateModel gameBoard,
            bool isSetupComplete,
            int wins)
        {
            CanFireShot = canFireShot;
            GameBoard = gameBoard;
            IsSetupComplete = isSetupComplete;
            Wins = wins;
        }


        public bool CanFireShot { get; }

        public GameBoardStateModel GameBoard { get; }

        public bool IsSetupComplete { get; }

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
                GameBoard,
                false,
                Wins);

        public PlayerStateModel CompleteSetup()
            => new PlayerStateModel(
                CanFireShot,
                GameBoard,
                true,
                Wins);

        public PlayerStateModel FireShot()
            => new PlayerStateModel(
                false,
                GameBoard,
                IsSetupComplete,
                Wins);

        public PlayerStateModel IncrementWins()
            => new PlayerStateModel(
                CanFireShot,
                GameBoard,
                IsSetupComplete,
                Wins + 1);

        public PlayerStateModel MoveShip(
                int shipIndex,
                Point shipSegment,
                Point targetPosition)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.MoveShip(shipIndex, shipSegment, targetPosition),
                IsSetupComplete,
                Wins);

        public PlayerStateModel ReceiveShot(
                Point position,
                IEnumerable<ShipDefinitionModel> ships)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.ReceiveShot(position, ships),
                IsSetupComplete,
                Wins);

        public PlayerStateModel RotateShip(
                int shipIndex,
                Point shipSegment,
                Rotation targetOrientation)
            => new PlayerStateModel(
                CanFireShot,
                GameBoard.RotateShip(shipIndex, shipSegment, targetOrientation),
                IsSetupComplete,
                Wins);

        public PlayerStateModel StartTurn()
            => new PlayerStateModel(
                true,
                GameBoard,
                IsSetupComplete,
                Wins);
    }
}
