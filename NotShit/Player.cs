using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotShit {
    class Player {
        private readonly GridDisplay _grid;
        public int X { get; private set; }
        public int Y { get; private set; }

        public Player(GridDisplay grid) {
            _grid = grid;
            X = grid.GridWidth / 2;
            Y = grid.GridHeight / 2;
        }

        public void Move(Direction direction) {
            switch (direction) {
                case Direction.Down:
                    Move(0, +1);
                    break;
                case Direction.DownLeft:
                    Move(-1, +1);
                    break;
                case Direction.DownRight:
                    Move(+1, +1);
                    break;
                case Direction.Left:
                    Move(-1, 0);
                    break;
                case Direction.Right:
                    Move(+1, 0);
                    break;
                case Direction.Up:
                    Move(0, -1);
                    break;
                case Direction.UpLeft:
                    Move(-1, -1);
                    break;
                case Direction.UpRight:
                    Move(+1, -1);
                    break;
            }
        }

        private void Move(int dx, int dy) {
            var newX = dx + X;
            var newY = dy + Y;

            if (newX < 0) {
                newX = _grid.GridWidth - 1 + dx;
            } else if (newX >= _grid.GridWidth) {
                newX = dx;
            }

            if (newY < 0) {
                newY = _grid.GridHeight - 1 + dy;
            } else if (newY >= _grid.GridHeight) {
                newY = dy;
            }

            X = newX;
            Y = newY;
        }
    }
}
