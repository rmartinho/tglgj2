namespace NotShit
{
    public struct Point
    {
        private int _x;
        private int _y;

        public Point(int x = 0, int y = 0) {
            _x = x;
            _y = y;
        }

        public int X {
            get { return _x; }
            set { _x = value; }
        }

        public int Y {
            get { return _y; }
            set { _y = value; }
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point {X = a.X + b.X, Y = a.Y + b.Y};
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point {X = a.X - b.X, Y = a.Y - b.Y};
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}