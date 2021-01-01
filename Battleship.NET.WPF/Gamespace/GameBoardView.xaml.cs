using System.Windows;
using System.Windows.Controls;

namespace Battleship.NET.WPF.Gamespace
{
    public partial class GameBoardView
        : UserControl
    {
        public DataTemplate BoardPositionTemplate
        {
            get => (DataTemplate)GetValue(BoardPositionTemplateProperty);
            set => SetValue(BoardPositionTemplateProperty, value);
        }
        public static readonly DependencyProperty BoardPositionTemplateProperty
            = DependencyProperty.Register(
                name:           nameof(BoardPositionTemplate),
                propertyType:   typeof(DataTemplate),
                ownerType:      typeof(GameBoardView));

        public object BoardPositionsOverlay
        {
            get => GetValue(BoardPositionsOverlayProperty);
            set => SetValue(BoardPositionsOverlayProperty, value);
        }
        public static readonly DependencyProperty BoardPositionsOverlayProperty
            = DependencyProperty.Register(
                name:           nameof(BoardPositionsOverlayProperty),
                propertyType:   typeof(object),
                ownerType:      typeof(GameBoardView));

        public GameBoardView()
            => InitializeComponent();
    }
}
