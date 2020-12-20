using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

using Battleship.NET.Domain.Models;

namespace Battleship.NET.Domain
{
    public class StandardGame
    {
        public static GameStateModel CreateIdle()
            => GameStateModel.CreateIdle(
                GameDefinitionModel.Create(
                    GameBoardDefinitionModel.Create("Standard", Enumerable.Range(0, 10).SelectMany(x => Enumerable.Range(0, 10).Select(y => new Point(x, y)))),
                    Enumerable.Empty<ShipDefinitionModel>()
                        .Append(ShipDefinitionModel.Create("Carrier",       Enumerable.Range(0, 5).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Battleship",    Enumerable.Range(0, 4).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Destroyer",     Enumerable.Range(0, 3).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Submarine",     Enumerable.Range(0, 3).Select(x => new Point(x, 0))))
                        .Append(ShipDefinitionModel.Create("Patrol Boat",   Enumerable.Range(0, 2).Select(x => new Point(x, 0))))
                        .ToImmutableArray(),
                    PlayerDefinitionModel.Create("Player 1"),
                    PlayerDefinitionModel.Create("Player 2")));
    }
}
