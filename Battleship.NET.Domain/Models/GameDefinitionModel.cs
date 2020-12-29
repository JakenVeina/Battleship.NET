using System.Collections.Generic;
using System.Collections.Immutable;

namespace Battleship.NET.Domain.Models
{
    public class GameDefinitionModel
    {
        public static GameDefinitionModel Create(
                GameBoardDefinitionModel gameBoard,
                IEnumerable<ShipDefinitionModel> ships,
                PlayerDefinitionModel player1,
                PlayerDefinitionModel player2)
            => new GameDefinitionModel(
                gameBoard:  gameBoard,
                player1:    player1,
                player2:    player2,
                ships:      ships.ToImmutableArray());

        private GameDefinitionModel(
            GameBoardDefinitionModel gameBoard,
            PlayerDefinitionModel player1,
            PlayerDefinitionModel player2,
            ImmutableArray<ShipDefinitionModel> ships)
        {
            GameBoard = gameBoard;
            Player1 = player1;
            Player2 = player2;
            Ships = ships;
        }

        public GameBoardDefinitionModel GameBoard { get; }

        public PlayerDefinitionModel Player1 { get; }

        public PlayerDefinitionModel Player2 { get; }

        public ImmutableArray<ShipDefinitionModel> Ships { get; }
    }
}
