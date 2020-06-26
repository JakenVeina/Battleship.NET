using System;
using System.Drawing;
using System.Linq;

namespace Battleship.NET.Domain.Models
{
    public class GameStateModel
    {
        public GameStateModel(
            GamePlayer currentPlayer,
            int gamesPlayed,
            DateTime lastUpdate,
            PlayerStateModel player1,
            PlayerStateModel player2,
            TimeSpan runtime,
            GameState state)
        {
            CurrentPlayer = currentPlayer;
            GamesPlayed = gamesPlayed;
            LastUpdate = lastUpdate;
            Player1 = player1;
            Player2 = player2;
            Runtime = runtime;
            State = state;
        }


        public GamePlayer CurrentPlayer { get; }

        public int GamesPlayed { get; }

        public DateTime LastUpdate { get; }

        public PlayerStateModel Player1 { get; }

        public PlayerStateModel Player2 { get; }

        public TimeSpan Runtime { get; }

        public GameState State { get; }


        public GameStateModel BeginSetup()
            => new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                LastUpdate,
                Player1.BeginSetup(),
                Player2.BeginSetup(),
                TimeSpan.Zero,
                GameState.Setup);

        public bool CanFireShot(
                Point position)
            => (State == GameState.Running)
                && (((CurrentPlayer == GamePlayer.Player1)
                        && Player1.CanFireShot
                        && Player2.CanReceiveShot(position))
                    || ((CurrentPlayer == GamePlayer.Player2)
                        && Player2.CanFireShot
                        && Player1.CanReceiveShot(position)));

        public bool CanCompleteSetup(
                GamePlayer player)
            => (State == GameState.Setup)
                && (((player == GamePlayer.Player1) && !Player1.CanCompleteSetup)
                    || ((player == GamePlayer.Player2) && !Player2.CanCompleteSetup));

        public bool CanMoveShip(
                GamePlayer player)
            => (State == GameState.Setup)
                && (((player == GamePlayer.Player1) && !Player1.IsSetupComplete)
                    || ((player == GamePlayer.Player2) && !Player2.IsSetupComplete));

        public bool CanStartGame()
            => (State == GameState.Ready);

        public bool CanTogglePause()
            => (State == GameState.Running)
                || (State == GameState.Paused);


        public GameStateModel CompleteSetup(
            GamePlayer player)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.CompleteSetup(), Player2)
                : (Player1, Player2.CompleteSetup());

            return new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                (player1.IsSetupComplete && player2.IsSetupComplete)
                    ? GameState.Ready
                    : State);
        }

        public GameStateModel EndTurn()
        {
            var players = new[] { Player1, Player2 };
            var (currentPlayerIndex, nextPlayerIndex, nextPlayer) = (CurrentPlayer == GamePlayer.Player1)
                ? (0, 1, GamePlayer.Player2)
                : (1, 0, GamePlayer.Player1);

            if (players[nextPlayerIndex].GameBoard.Hits.Count == players[currentPlayerIndex].GameBoard.Ships.Sum(ship => ship.Definition.Points.Count))
            {
                players[currentPlayerIndex] = players[currentPlayerIndex].IncrementWins();

                return new GameStateModel(
                    CurrentPlayer,
                    GamesPlayed + 1,
                    LastUpdate,
                    players[0],
                    players[1],
                    Runtime,
                    GameState.Complete);
            }

            players[nextPlayerIndex] = players[nextPlayerIndex].StartTurn();

            return new GameStateModel(
                nextPlayer,
                GamesPlayed,
                LastUpdate,
                players[0],
                players[1],
                Runtime,
                State);
        }

        public GameStateModel FireShot(
            Point position)
        {
            var (player1, player2) = (CurrentPlayer == GamePlayer.Player1)
                ? (Player1.FireShot(), Player2.ReceiveShot(position))
                : (Player1.ReceiveShot(position), Player2.FireShot());

            return new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                State);
        }

        public GameStateModel MoveShip(
            GamePlayer player,
            int shipIndex,
            Rotation orientation,
            Point position)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.MoveShip(shipIndex, orientation, position), Player2)
                : (Player1, Player2.MoveShip(shipIndex, orientation, position));

            return new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                State);
        }

        public GameStateModel StartGame(
            GamePlayer firstPlayer)
        {
            var (player1, player2) = (firstPlayer == GamePlayer.Player1)
                ? (Player1.StartTurn(), Player2)
                : (Player1, Player2.StartTurn());

            return new GameStateModel(
                firstPlayer,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                GameState.Running);
        }

        public GameStateModel TogglePause()
            => new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                LastUpdate,
                Player1,
                Player2,
                Runtime,
                (State == GameState.Paused)
                    ? GameState.Running
                    : GameState.Paused);

        public GameStateModel UpdateRuntime(
                DateTime now)
            => new GameStateModel(
                CurrentPlayer,
                GamesPlayed,
                now,
                Player1,
                Player2,
                (State == GameState.Running)
                    ? (Runtime + (now - LastUpdate))
                    : Runtime,
                State);
    }
}