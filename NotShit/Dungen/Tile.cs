using System;

namespace NotShit.Dungen
{
    public class Tile
    {
        private Mob _mob;
        public TileKind Kind { get; set; }

        public Mob Mob {
            get { return _mob; }
            set {
                if (value != null && _mob != null) {
                    throw new Exception("You need to move the old mob before putting a new one here!");
                }
                if (Kind == TileKind.Wall && value != null) {
                    throw new Exception("Can't put a mob in a wall!");
                }
                _mob = value;
            }
        }

        public override string ToString()
        {
            return Kind.ToString();
        }

        public char ToChar()
        {
            switch (Kind)
            {
                case TileKind.Wall:
                    return '#';
                case TileKind.Floor:
                    return ' ';
            }
            throw new Exception(string.Format("I have no idea what the fuck kind of tile this was: {0}", Kind.ToString()));
        }
    }
}