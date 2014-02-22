namespace NotShit
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Point operator +(Point a, Point b)
        {
            return new Point {X = a.X + b.X, Y = a.Y + b.Y};
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point {X = a.X - b.X, Y = a.Y - b.Y};
        }

        public static Point operator /(Point a, int d)
        {
            return new Point {X = a.X/d, Y = a.Y/d};
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}