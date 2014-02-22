using System;
using System.Collections.Generic;
using System.Linq;

namespace NotShit.Dungen
{
    public enum TileKind
    {
        Wall,
        Floor
    }

    public class Room
    {
        public Point TopLeft { get; set; }
        public Point Size { get; set; }

        public Point BottomRight
        {
            get { return TopLeft + Size; }
            set { Size = value - TopLeft; }
        }

        public bool Intersects(Room other)
        {
            return (TopLeft.X > other.BottomRight.X || BottomRight.X < other.TopLeft.X)
                   && (TopLeft.Y > other.BottomRight.Y || BottomRight.Y < other.TopLeft.Y);
        }

        public bool IntersectsWithWalls(Room other)
        {
            return !((TopLeft.X > other.BottomRight.X + 1 || BottomRight.X + 1 < other.TopLeft.X)
                     && (TopLeft.Y > other.BottomRight.Y + 1 || BottomRight.Y + 1 < other.TopLeft.Y));
        }

        public bool Contains(Point point)
        {
            return point.X >= TopLeft.X && point.X <= BottomRight.X
                   && point.Y >= TopLeft.Y && point.Y <= BottomRight.Y;
        }

        public bool ContainsWithWalls(Point point)
        {
            return point.X >= TopLeft.X - 1 && point.X <= BottomRight.X + 1
                   && point.Y >= TopLeft.Y - 1 && point.Y <= BottomRight.Y + 1;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", TopLeft, BottomRight);
        }
    }

    public class Level
    {
        private readonly List<Room> _rooms;
        private readonly List<Tile> _tiles;

        public Level(int width, int height, int nRooms, int minSize, int maxSize)
        {
            Width = width;
            Height = height;
            _tiles = new List<Tile>(Infinite(() => new Tile {Kind = TileKind.Wall}).Take(width*height));
            _rooms = new List<Room>();

            Func<Room, bool> shouldAdd = r => !_rooms.Any(existingRoom => existingRoom.IntersectsWithWalls(r));
            Infinite(() => new Room
            {
                TopLeft = GenGod.Point(0, 0, width, height),
                Size = GenGod.Point(minSize, minSize, maxSize, maxSize)
            }).Select(r =>
            {
                int adjustedX = r.BottomRight.X >= Width ? Width-1 : r.BottomRight.X;
                int adjustedY = r.BottomRight.Y >= Height ? Height-1 : r.BottomRight.Y;
                r.BottomRight = new Point {X = adjustedX, Y = adjustedY};
                return r;
            }).Take(nRooms * 10000)
            .Where(r =>
            {
                if (shouldAdd(r))
                {
                    _rooms.Add(r);
                    return true;
                }
                return false;
            }).Take(nRooms)
            .ToList(); // Consume and discard; output is side-effected into _rooms


            foreach (Room room in _rooms)
            {
                CarveRoom(room);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Tile this[Point index]
        {
            get { return _tiles[index.X + index.Y*Width]; }
            set { _tiles[index.X + index.Y*Width] = value; }
        }

        private void CarveRoom(Room room)
        {
            IEnumerable<Point> positions = from x in Enumerable.Range(room.TopLeft.X, room.Size.X)
                from y in Enumerable.Range(room.TopLeft.Y, room.Size.Y)
                select new Point {X = x, Y = y};
            foreach (Point position in positions)
            {
                this[position] = new Tile {Kind = TileKind.Floor};
            }
        }

        private static IEnumerable<T> Infinite<T>(Func<T> gen)
        {
            while (true)
                yield return gen();
        }

        public IEnumerable<Point> Positions()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    yield return new Point {X = x, Y = y};
                }
            }
        }

        public void Draw(GridDisplay grid)
        {
            foreach (Point position in Positions())
            {
                grid.Put(this[position].ToChar(), position.X, position.Y);
            }
        }
    }
}