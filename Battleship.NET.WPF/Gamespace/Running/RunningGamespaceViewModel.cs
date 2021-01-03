using System.Reactive;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Running
{
    public class RunningGamespaceViewModel
        : GameBoardViewModelBase<RunningGamespaceBoardPositionViewModel>
    {
        public RunningGamespaceViewModel(
                GameBoardColumnHeadingsViewModel columnHeadings,
                GameBoardRowHeadingsViewModel rowHeadings)
            : base(
                columnHeadings,
                rowHeadings)
        {
            // TODO: Implement this
            EndTurnCommand = ReactiveCommand.Create(() => { });

            // TODO: Implement this
            TogglePauseCommand = ReactiveCommand.Create(() => { });
        }

        public ICommand<Unit> EndTurnCommand { get; }

        public ICommand<Unit> TogglePauseCommand { get; }
    }
}
