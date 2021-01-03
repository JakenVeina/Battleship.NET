using System.Drawing;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceBoardPositionViewModelFactory
    {
        public CompletedGamespaceBoardPositionViewModel Create(
                Point position)
            => new CompletedGamespaceBoardPositionViewModel(
                position);
    }
}
