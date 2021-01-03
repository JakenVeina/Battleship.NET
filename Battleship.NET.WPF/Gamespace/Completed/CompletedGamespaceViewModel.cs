using System.Reactive;
using System.Windows.Input;

namespace Battleship.NET.WPF.Gamespace.Completed
{
    public class CompletedGamespaceViewModel
        : GameBoardViewModelBase<CompletedGamespaceBoardPositionViewModel>
    {
        public CompletedGamespaceViewModel(
                GameBoardColumnHeadingsViewModel columnHeadings,
                GameBoardRowHeadingsViewModel rowHeadings)
            : base(
                columnHeadings,
                rowHeadings)
        {
            // TODO: Implement this
            BeginSetupCommand = ReactiveCommand.Create(() => { });

            // TODO: Implement this
            ToggleActivePlayerCommand = ReactiveCommand.Create(() => { });
        }

        public ICommand<Unit> BeginSetupCommand { get; }

        public ICommand<Unit> ToggleActivePlayerCommand { get; }
    }
}
