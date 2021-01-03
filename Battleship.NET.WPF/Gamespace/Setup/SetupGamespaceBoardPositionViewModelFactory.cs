using System.Drawing;

namespace Battleship.NET.WPF.Gamespace.Setup
{
    public class SetupGamespaceBoardPositionViewModelFactory
    {
        public SetupGamespaceBoardPositionViewModel Create(
                Point position)
            => new SetupGamespaceBoardPositionViewModel(
                position);
    }
}
