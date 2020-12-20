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
                Orientation rotation)
            => rotation switch
            {
                Orientation.Rotate90    => new Point(point.Y, -point.X),
                Orientation.Rotate180   => new Point(-point.X, -point.Y),
                Orientation.Rotate270   => new Point(-point.Y, point.X),
                _                       => point
            };

        public static Point Translate(
                this Point point,
                Point translation)
            => new Point(
                point.X + translation.X,
                point.Y + translation.Y);
    }
}
