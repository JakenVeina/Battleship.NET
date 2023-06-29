using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace Battleship.NET.Domain.Models
{
    public class GameStateModel
    {
        public static GameStateModel CreateIdle(
                GameDefinitionModel definition)
            => new GameStateModel(
                currentPlayer:  default,
                definition:     definition,
                gamesPlayed:    0,
                lastUpdate:     default,
                phase:          GamePhase.Idle,
                player1:        PlayerStateModel.CreateIdle(definition.Ships),
                player2:        PlayerStateModel.CreateIdle(definition.Ships));

        private GameStateModel(
            GamePlayer currentPlayer,
            GameDefinitionModel definition,
            int gamesPlayed,
            DateTime lastUpdate,
            GamePhase phase,
            PlayerStateModel player1,
            PlayerStateModel player2)
        {
            CurrentPlayer   = currentPlayer;
            Definition      = definition;
            GamesPlayed     = gamesPlayed;
            LastUpdate      = lastUpdate;
            Phase           = phase;
            Player1         = player1;
            Player2         = player2;
        }

        private GameStateModel(GameStateModel original)
        {
            CurrentPlayer   = original.CurrentPlayer;
            Definition      = original.Definition;
            GamesPlayed     = original.GamesPlayed;
            LastUpdate      = original.LastUpdate;
            Phase           = original.Phase;
            Player1         = original.Player1;
            Player2         = original.Player2;
        }

        public GamePlayer CurrentPlayer { get; private init; }

        public GameDefinitionModel Definition { get; private init; }

        public int GamesPlayed { get; private init; }

        public DateTime LastUpdate { get; private init; }

        public GamePhase Phase { get; private init; }

        public PlayerStateModel Player1 { get; private init; }

        public PlayerStateModel Player2 { get; private init; }


        public GameStateModel BeginSetup()
            => new(this)
            {
                Phase   = GamePhase.Setup,
                Player1 = Player1.BeginSetup(),
                Player2 = Player2.BeginSetup()
            };

        public GameStateModel CompleteGame(
            GamePlayer winner)
        {
            var (player1, player2) = (winner == GamePlayer.Player1)
                ? (Player1.IncrementWins(), Player2)
                : (Player1, Player2.IncrementWins());

            return new(this)
            {
                GamesPlayed = GamesPlayed + 1,
                Phase       = GamePhase.Complete,
                Player1     = player1,
                Player2     = player2
            };
        }

        public GameStateModel CompleteSetup(
            GamePlayer player)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.CompleteSetup(), Player2)
                : (Player1, Player2.CompleteSetup());

            return new(this)
            {
                Phase   = (player1.IsSetupComplete && player2.IsSetupComplete)
                    ? GamePhase.Ready
                    : Phase,
                Player1 = player1,
                Player2 = player2
            };
        }

        public GameStateModel EndTurn()
        {
            var (player1, player2, nextPlayer) = (CurrentPlayer == GamePlayer.Player1)
                ? (Player1, Player2.StartTurn(), GamePlayer.Player2)
                : (Player1.StartTurn(), Player2, GamePlayer.Player1);

            return new(this)
            {
                CurrentPlayer   = nextPlayer,
                Player1         = player1,
                Player2         = player2
            };
        }

        public GameStateModel FireShot(
            Point position)
        {
            PlayerStateModel player1, player2;
            if (CurrentPlayer == GamePlayer.Player1)
            {
                var shotOutcome = GetShotOutcome(Player2.GameBoard.Ships);

                player1 = Player1.FireShot(shotOutcome);
                player2 = Player2.ReceiveShot(position, shotOutcome);
            }
            else
            {
                var shotOutcome = GetShotOutcome(Player1.GameBoard.Ships);

                player2 = Player2.FireShot(shotOutcome);
                player1 = Player1.ReceiveShot(position, shotOutcome);
            }

            return new(this)
            {
                Player1 = player1,
                Player2 = player2
            };

            ShotOutcome GetShotOutcome(ImmutableList<ShipStateModel> shipStates)
                => Enumerable.Zip(
                            Definition.Ships,
                            shipStates,
                            (definition, state) => definition.Segments
                                .Select(segment => segment
                                    .RotateOrigin(state.Orientation)
                                    .Translate(state.Position)))
                        .SelectMany(positions => positions)
                        .Contains(position)
                    ? ShotOutcome.Hit
                    : ShotOutcome.Miss;
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

            return new(this)
            {
                Player1 = player1,
                Player2 = player2,
            };
        }

        public GameStateModel RandomizeShips(
            GamePlayer player,
            Random random)
        {
            var boardSize = Selectors.BoardSelectors.Size.Invoke(this);

            GameStateModel state;
            do
            {
                var (player1, player2) = (player == GamePlayer.Player1)
                    ? (RandomizeShips(Player1), Player2)
                    : (Player1, RandomizeShips(Player2));

                state = new(this)
                {
                    Player1 = player1,
                    Player2 = player2
                };
            } while (!Selectors.BoardSelectors.IsValid[player].Invoke(state));

            return state;

            PlayerStateModel RandomizeShips(PlayerStateModel player)
                => player.PlaceShips(Enumerable.Range(0, Definition.Ships.Length)
                    .Select(_ => ShipStateModel.Place(
                        orientation:    (Orientation)(random.Next(0, 4) * 90),
                        position:       new Point(
                            random.Next(0, boardSize.Width),
                            random.Next(0, boardSize.Height))))
                    .ToImmutableList());
        }

        public GameStateModel RotateShip(
            GamePlayer player,
            int shipIndex,
            Point shipSegment,
            Orientation targetOrientation)
        {
            var (player1, player2) = (player == GamePlayer.Player1)
                ? (Player1.RotateShip(shipIndex, shipSegment, targetOrientation), Player2)
                : (Player1, Player2.RotateShip(shipIndex, shipSegment, targetOrientation));

            return new(this)
            {
                Player1 = player1,
                Player2 = player2,
            };
        }

        public GameStateModel StartGame(
            GamePlayer firstPlayer)
        {
            var (player1, player2) = (firstPlayer == GamePlayer.Player1)
                ? (Player1.StartTurn(), Player2)
                : (Player1, Player2.StartTurn());

            return new(this)
            {
                CurrentPlayer   = firstPlayer,
                Phase           = GamePhase.Running,
                Player1         = player1,
                Player2         = player2
            };
        }

        public GameStateModel TogglePause()
            => new(this)
            {
                Phase = (Phase == GamePhase.Paused)
                    ? GamePhase.Running
                    : GamePhase.Paused
            };

        public GameStateModel UpdateRuntime(
            DateTime now)
        {
            var (player1, player2) = (Phase == GamePhase.Running)
                ? (CurrentPlayer == GamePlayer.Player1)
                    ? (Player1.IncrementPlayTime(now - LastUpdate), Player2)
                    : (Player1, Player2.IncrementPlayTime(now - LastUpdate))
                : (Player1, Player2);

            return new(this)
            {
                LastUpdate  = now,
                Player1     = player1,
                Player2     = player2
            };
        }
    }
}
