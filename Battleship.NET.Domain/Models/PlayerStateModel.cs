using System.Drawing;
using System.Net;

namespace Battleship.NET.Domain.Models
{
    public class PlayerStateModel
    {
        public PlayerStateModel(
            bool canFireShot,
            PlayerDefinitionModel definition,
            GameBoardStateModel gameBoard,
            bool isSetupComplete,
            int wins)
        {
            CanFireShot = canFireShot;
            Definition = definition;
            GameBoard = gameBoard;
            IsSetupComplete = isSetupComplete;
            Wins = wins;
        }


        public bool CanFireShot { get; }

        public PlayerDefinitionModel Definition { get; }

        public GameBoardStateModel GameBoard { get; }

        public bool IsSetupComplete { get; }

        public int Wins { get; }


        public bool CanCompleteSetup
            => !IsSetupComplete
                && GameBoard.IsValid;


        public bool CanReceiveShot(
                Point position)
            => GameBoard.CanReceiveShot(position);


        public PlayerStateModel BeginSetup()
            => new PlayerStateModel(
                CanFireShot,
                Definition,
                GameBoard,
                false,
                Wins);

        public PlayerStateModel CompleteSetup()
            => new PlayerStateModel(
                CanFireShot,
                Definition,
                GameBoard,
                true,
                Wins);

        public PlayerStateModel FireShot()
            => new PlayerStateModel(
                false,
                Definition,
                GameBoard,
                IsSetupComplete,
                Wins);

        public PlayerStateModel IncrementWins()
            => new PlayerStateModel(
                CanFireShot,
                Definition,
                GameBoard,
                IsSetupComplete,
                Wins + 1);

        public PlayerStateModel MoveShip(
                int shipIndex,
                Rotation orientation,
                Point position)
            => new PlayerStateModel(
                CanFireShot,
                Definition,
                GameBoard.MoveShip(shipIndex, orientation, position),
                IsSetupComplete,
                Wins);

        public PlayerStateModel ReceiveShot(
                Point position)
            => new PlayerStateModel(
                CanFireShot,
                Definition,
                GameBoard.ReceiveShot(position),
                IsSetupComplete,
                Wins);

        public PlayerStateModel StartTurn()
            => new PlayerStateModel(
                true,
                Definition,
                GameBoard,
                IsSetupComplete,
                Wins);
    }
}
