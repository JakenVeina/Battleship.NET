using System;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain.Actions
{
    public class RandomizeShipsAction
        : IAction
    {
        public RandomizeShipsAction(
            GamePlayer player,
            Random random)
        {
            Player = player;
            Random = random;
        }

        public GamePlayer Player { get; }

        public Random Random { get; }
    }
}
