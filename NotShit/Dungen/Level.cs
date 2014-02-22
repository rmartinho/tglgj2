using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;

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


        public bool IsWestOf(Room other)
        {
            return TopLeft.X < other.TopLeft.X;
        }
        public bool IsEastOf(Room other)
        {
            return TopLeft.X > other.TopLeft.X;
        }
        public bool IsNorthOf(Room other)
        {
            return TopLeft.Y < other.TopLeft.Y;
        }
        public bool IsSouthOf(Room other)
        {
            return TopLeft.Y > other.TopLeft.Y;
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

        public Point GetWalkablePoint()
        {
            return _rooms[0].TopLeft + _rooms[0].Size/2;
        }

        public Level(int width, int height, int nRooms, int minSize, int maxSize)
        {
            Width = width;
            Height = height;
            _tiles = new List<Tile>(Infinite(() => new Tile {Kind = TileKind.Wall}).Take(width*height));

            _rooms = Infinite(() => GenGod.Point(0, 0, Width, Height))
                .Where(p => this[p].Kind == TileKind.Wall)
                .Where(p =>
                {
                    var growth = PossibleGrowth(p);
                    return growth.X > 0 && growth.Y > 0;
                })
                .Select(p =>
                {
                    Point growth = PossibleGrowth(p);
                    var half = new Point {X = growth.X/2, Y = growth.Y/2};
                    var room = new Room
                    {
                        TopLeft = p + half,
                        Size = growth
                    };
                    var clampedX = room.BottomRight.X >= Width ? Width - 1 : room.BottomRight.X;
                    var clampedY = room.BottomRight.Y >= Height ? Height - 1 : room.BottomRight.Y;
                    room.BottomRight = new Point {X = clampedX, Y = clampedY};
                    CarveRoom(room);
                    return room;
                })
                .Take(nRooms)
                .ToList();

            var connections = (from a in _rooms
                from b in _rooms
                where a != b && a.IsWestOf(b)
                select new {a, b}).ToList();

            foreach (var c in connections)
            {
                foreach (var p in EastOf(c.a.TopLeft).TakeWhile(p => p.X <= c.b.TopLeft.X))
                {
                    this[p].Kind = TileKind.Floor;
                }
                foreach (var p in NorthOf(new Point { X = c.b.TopLeft.X, Y = c.a.TopLeft.Y }).TakeWhile(p => p.Y >= c.b.TopLeft.Y))
                {
                    this[p].Kind = TileKind.Floor;
                }
                foreach (var p in SouthOf(new Point { X = c.b.TopLeft.X, Y = c.a.TopLeft.Y }).TakeWhile(p => p.Y <= c.b.TopLeft.Y))
                {
                    this[p].Kind = TileKind.Floor;
                }
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public List<Room> Rooms {
            get { return _rooms; }
        }

        public Tile this[Point index]
        {
            get { return _tiles[index.X + index.Y*Width]; }
            set { _tiles[index.X + index.Y*Width] = value; }
        }

        private IEnumerable<Point> WestOf(Point at)
        {
            for (int x = at.X-1; x > 0; x--)
            {
                yield return new Point {X = x, Y = at.Y};
            }
        }

        private IEnumerable<Point> EastOf(Point at)
        {
            for (int x = at.X+1; x < Width - 1; x++)
            {
                yield return new Point {X = x, Y = at.Y};
            }
        }

        private IEnumerable<Point> NorthOf(Point at)
        {
            for (int y = at.Y-1; y > 0; y--)
            {
                yield return new Point {X = at.X, Y = y};
            }
        }

        private IEnumerable<Point> SouthOf(Point at)
        {
            for (int y = at.Y+1; y < Height - 1; y++)
            {
                yield return new Point {X = at.X, Y = y};
            }
        }

        private Point PossibleGrowth(Point at)
        {
            int maxWest = WestOf(at).TakeWhile(p => this[p].Kind == TileKind.Wall).Count() - 1;
            int maxEast = EastOf(at).TakeWhile(p => this[p].Kind == TileKind.Wall).Count() - 1;
            int maxNorth = NorthOf(at).TakeWhile(p => this[p].Kind == TileKind.Wall).Count() -1;
            int maxSouth = SouthOf(at).TakeWhile(p => this[p].Kind == TileKind.Wall).Count() - 1;
            return new Point {X = Math.Min(maxWest, maxEast), Y = Math.Min(maxNorth, maxSouth)};
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
                grid.Put(this[position].ToChar(), position.X, position.Y, new Color(255, 255, 255));
            }
        }
    }
}