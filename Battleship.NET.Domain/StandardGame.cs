using System.Drawing;
using System.Linq;

using Redux;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain
{
    public static class StandardGame
    {
        public static GameStateStore Create(
                params Middleware<GameStateModel>[] middlewares)
            => new GameStateStore(
                GameStateModel.CreateIdle(
                    GameBoardDefinitionModel.Create("Standard", Enumerable.Range(0, 10).SelectMany(x => Enumerable.Range(0, 10).Select(y => new Point(x, y)))),
                    Enumerable.Empty<ShipDefinitionModel>()
                        .Append(ShipDefinitionModel.Create("Carrier",       Enumerable.Range(0, 5).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Battleship",    Enumerable.Range(0, 4).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Destroyer",     Enumerable.Range(0, 3).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Submarine",     Enumerable.Range(0, 3).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Patrol Boat",   Enumerable.Range(0, 2).Select(x => new Point(x, 0)))),
                    new PlayerDefinitionModel("Player 1"),
                    new PlayerDefinitionModel("Player 2")),
                middlewares);
    }
}
