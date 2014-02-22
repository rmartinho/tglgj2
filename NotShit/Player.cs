using System;
using System.Collections;
using System.Collections.Generic;

namespace NotShit {
    class Player {
        private readonly GridDisplay _grid;
        private readonly Queue<string> _messages;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public bool HasMessages { get { return _messages.Count > 0; } }
        public string CurrentMessage { get { return _messages.Peek(); } }

        public Player(GridDisplay grid) {
            _grid = grid;
            X = grid.GridWidth / 2;
            Y = grid.GridHeight / 2;

            Health = MaxHealth = 25;

            _messages = new Queue<string>();
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

        public void DismissMessage() {
            _messages.Dequeue();
        }

        public void AddMessage(string message) {
            if (message.Length >= _grid.GridWidth - " [CONT]".Length) {
                throw new Exception("Message too long.");
            }
            _messages.Enqueue(message);
        }

        public void Draw() {
            var health = (double)Health / MaxHealth;
            byte r = 0,
                 g = 0,
                 b = 0;
            
            if (health >= 0.9) {
                g = 200;
            } else if (health >= 0.5) {
                r = 200;
                g = 200;
            } else {
                r = 200;
            }

            _grid.Put('@', X, Y, r, g, b);
        }

        public void Damage(int damage) {
            AddMessage("A mysterious force smites you!");
            Health -= damage;
        }
    }
}
