using System.Drawing;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceShipSegmentViewModelFactory
    {
        public SetupGamespaceShipSegmentViewModel Create(
                Point segment,
                int shipIndex)
            => new SetupGamespaceShipSegmentViewModel(
                segment,
                shipIndex);
    }
}
