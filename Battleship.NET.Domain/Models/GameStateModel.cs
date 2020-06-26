using System;

namespace Battleship.NET.Domain.Models
{
    public class GameStateModel
    {
        public GameStateModel(
            GamePlayer currentPlayer,
            GamePlayer firstPlayer,
            int gamesPlayed,
            DateTimeOffset lastUpdate,
            PlayerStateModel player1,
            PlayerStateModel player2,
            TimeSpan runtime,
            GameState state)
        {
            CurrentPlayer = currentPlayer;
            FirstPlayer = firstPlayer;
            GamesPlayed = gamesPlayed;
            LastUpdate = lastUpdate;
            Player1 = player1;
            Player2 = player2;
            Runtime = runtime;
            State = state;
        }

        public GamePlayer CurrentPlayer { get; }

        public GamePlayer FirstPlayer { get; }

        public int GamesPlayed { get; }

        public DateTimeOffset LastUpdate { get; }

        public PlayerStateModel Player1 { get; }
        
        public PlayerStateModel Player2 { get; }

        public TimeSpan Runtime { get; }

        public GameState State { get; }
    }
}
