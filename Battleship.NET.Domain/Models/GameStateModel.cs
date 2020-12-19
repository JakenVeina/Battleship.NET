using System;
using System.Drawing;

namespace Battleship.NET.Domain.Models
{
    public class GameStateModel
    {
        public static GameStateModel CreateIdle(
                GameDefinitionModel definition)
            => new GameStateModel(
                default,
                definition,
                0,
                default,
                PlayerStateModel.CreateIdle(definition.Ships),
                PlayerStateModel.CreateIdle(definition.Ships),
                TimeSpan.Zero,
                GameState.Idle);

        public GameStateModel(
            GamePlayer currentPlayer,
            GameDefinitionModel definition,
            int gamesPlayed,
            DateTime lastUpdate,
            PlayerStateModel player1,
            PlayerStateModel player2,
            TimeSpan runtime,
            GameState state)
        {
            CurrentPlayer = currentPlayer;
            Definition = definition;
            GamesPlayed = gamesPlayed;
            LastUpdate = lastUpdate;
            Player1 = player1;
            Player2 = player2;
            Runtime = runtime;
            State = state;
        }


        public GamePlayer CurrentPlayer { get; }

        public GameDefinitionModel Definition { get; }

        public int GamesPlayed { get; }

        public DateTime LastUpdate { get; }

        public PlayerStateModel Player1 { get; }

        public PlayerStateModel Player2 { get; }

        public TimeSpan Runtime { get; }

        public GameState State { get; }


        public PlayerStateModel CurrentPlayerState
            => (CurrentPlayer == GamePlayer.Player1)
                ? Player1
                : Player2;

        public PlayerStateModel OpponentPlayerState
            => (CurrentPlayer == GamePlayer.Player2)
                ? Player1
                : Player2;

        public GameStateModel BeginSetup()
            => new GameStateModel(
                CurrentPlayer,
                Definition,
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
                && !(((player == GamePlayer.Player1) && !Player1.CanCompleteSetup(Definition.GameBoard, Definition.Ships))
                    || ((player == GamePlayer.Player2) && !Player2.CanCompleteSetup(Definition.GameBoard, Definition.Ships)));

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


        public GameStateModel CompleteGame(
            GamePlayer winner)
        {
            var (player1, player2) = (winner == GamePlayer.Player1)
                ? (Player1.IncrementWins(), Player2)
                : (Player1, Player2.IncrementWins());

            return new GameStateModel(
                CurrentPlayer,
                Definition,
                GamesPlayed + 1,
                LastUpdate,
                player1,
                player2,
                Runtime,
                GameState.Complete);
        }

        public GameStateModel CompleteSetup(
            GamePlayer player)
        {
            var (player1, player2, nextPlayer) = (player == GamePlayer.Player1)
                ? (Player1.CompleteSetup(), Player2, GamePlayer.Player2)
                : (Player1, Player2.CompleteSetup(), GamePlayer.Player1);

            return new GameStateModel(
                nextPlayer,
                Definition,
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
            var (player1, player2, nextPlayer) = (CurrentPlayer == GamePlayer.Player1)
                ? (Player1, Player2.StartTurn(), GamePlayer.Player2)
                : (Player1.StartTurn(), Player2, GamePlayer.Player1);

            return new GameStateModel(
                nextPlayer,
                Definition,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                State);
        }

        public GameStateModel FireShot(
            Point position)
        {
            PlayerStateModel player1, player2;
            if (CurrentPlayer == GamePlayer.Player1)
            {
                player2 = Player2.ReceiveShot(position, Definition.Ships);
                player1 = Player1.FireShot(player2.GameBoard.Hits.Contains(position));
            }
            else
            {
                player1 = Player1.ReceiveShot(position, Definition.Ships);
                player2 = Player2.FireShot(player1.GameBoard.Hits.Contains(position));
            }

            return new GameStateModel(
                CurrentPlayer,
                Definition,
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
            Point shipSegment,
            Point targetPosition)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.MoveShip(shipIndex, shipSegment, targetPosition), Player2)
                : (Player1, Player2.MoveShip(shipIndex, shipSegment, targetPosition));

            return new GameStateModel(
                CurrentPlayer,
                Definition,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                State);
        }

        public GameStateModel RandomizeShips(
            GamePlayer player,
            Random random)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.RandomizeShips(Definition.GameBoard, Definition.Ships, random), Player2)
                : (Player1, Player2.RandomizeShips(Definition.GameBoard, Definition.Ships, random));

            return new GameStateModel(
                CurrentPlayer,
                Definition,
                GamesPlayed,
                LastUpdate,
                player1,
                player2,
                Runtime,
                State);
        }

        public GameStateModel RotateShip(
            GamePlayer player,
            int shipIndex,
            Point shipSegment,
            Rotation targetOrientation)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.RotateShip(shipIndex, shipSegment, targetOrientation), Player2)
                : (Player1, Player2.RotateShip(shipIndex, shipSegment, targetOrientation));

            return new GameStateModel(
                CurrentPlayer,
                Definition,
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
                Definition,
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
                Definition,
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
                Definition,
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