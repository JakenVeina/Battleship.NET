namespace Battleship.NET.Domain.Models
{
    public class PlayerStateModel
    {
        public PlayerStateModel(
            bool canFireShot,
            PlayerDefinitionModel definition,
            GameBoardStateModel gameBoard,
            int wins)
        {
            CanFireShot = canFireShot;
            Definition = definition;
            GameBoard = gameBoard;
            Wins = wins;
        }

        public bool CanFireShot { get; }

        public PlayerDefinitionModel Definition { get; }

        public GameBoardStateModel GameBoard { get; }

        public int Wins { get; }
    }
}
