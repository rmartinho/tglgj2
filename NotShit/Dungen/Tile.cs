using System;

namespace NotShit.Dungen
{
    public class Tile
    {
        public TileKind Kind { get; set; }

        public override string ToString()
        {
            return Kind.ToString();
        }

        public char ToChar()
        {
            switch (Kind)
            {
                case TileKind.Wall:
                    return 'x';
                case TileKind.Floor:
                    return ' ';
            }
            throw new Exception(string.Format("I have no idea what the fuck kind of tile this was: {0}", Kind.ToString()));
        }
    }
}