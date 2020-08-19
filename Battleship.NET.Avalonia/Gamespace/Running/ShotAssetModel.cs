namespace Battleship.NET.Avalonia.Gamespace.Running
{
    public class ShotAssetModel
    {
        public ShotAssetModel(
            bool isHit)
        {
            IsHit = isHit;
        }

        public bool IsHit { get; }
    }
}
