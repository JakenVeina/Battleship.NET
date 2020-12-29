using System.Drawing;

namespace Battleship.NET.WPF.Gamespace
{
    public class GameBoardPositionViewModelBase
    {
        protected GameBoardPositionViewModelBase(
            Point position)
        {
            Position = position;
        }

        public Point Position { get; }
    }
}
