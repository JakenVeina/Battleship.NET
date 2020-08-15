namespace System.Drawing
{
    public static class PointExtensions
    {
        public static Point Negate(
                this Point point)
            => new Point(
                -point.X,
                -point.Y);

        public static Point RotateOrigin(
                this Point point,
                Rotation rotation)
            => rotation switch
            {
                Rotation.Rotate90   => new Point(point.Y, -point.X),
                Rotation.Rotate180  => new Point(-point.X, -point.Y),
                Rotation.Rotate270  => new Point(-point.Y, point.X),
                _                   => point
            };

        public static Point Translate(
                this Point point,
                Point translation)
            => new Point(
                point.X + translation.X,
                point.Y + translation.Y);
    }
}
