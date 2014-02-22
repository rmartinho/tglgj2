using System.Globalization;

namespace NotShit {
    public class GridDisplay {
        private class Tile {
            public char Character { get; set; }
            public Color Color { get; set; }

            public Tile() {
                Color = Color.White;
                Character = ' ';
            }
        }

        private readonly Display _display;
        private readonly Font _font;
        private readonly Tile[,] _grid;

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }

        public GridDisplay(Display display, Font font) {
            // will work only with monospace
            int x, y, w, h;
            
            font.Measure("@", out x, out y, out w, out h);

            TileWidth = w;
            TileHeight = h;

            GridWidth = display.Width / TileWidth;
            GridHeight = display.Height / TileHeight;

            _font = font;
            _display = display;
            _grid = new Tile[GridWidth, GridHeight];
        }

        public void Clear() {
            for (var x = 0; x < GridWidth; ++x) {
                for (var y = 0; y < GridHeight; ++y) {
                    _grid[x, y] = new Tile();
                }
            }
        }

        public void Put(char ch, int x, int y, Color color) {
            _grid[x, y] = new Tile {
                Color = color,
                Character = ch
            };
        }

        public void Draw() {
            for (var x = 0; x < GridWidth; ++x) {
                for (var y = 0; y < GridHeight; ++y) {
                    var tile = _grid[x, y];
                    var str = tile.Character.ToString(CultureInfo.InvariantCulture);
                    _font.Draw(str, TileWidth * x, TileHeight * y, tile.Color);
                }
            }
        }
    }
}
