using System.Drawing;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceBoardPositionViewModelFactory
    {
        public RunningGamespaceBoardPositionViewModel Create(
                Point position)
            => new RunningGamespaceBoardPositionViewModel(
                position);
    }
}
