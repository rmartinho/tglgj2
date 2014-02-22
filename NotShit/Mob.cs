using System;

namespace NotShit {
    public class Mob {
        protected readonly GridDisplay _grid;

        public virtual int MaxHealth { get; set; }
        public virtual int Health { get; set; }
        public bool Alive { get { return Health > 0; } }
        public virtual int Attack { get; set; }
        public virtual int Defense { get; set; }
        public virtual string Name { get; set; }
        public virtual Color Color { get; set; }
        public virtual char Tile { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Mob LastAttack { get; set; }

        public Mob(GridDisplay grid, MobTemplate template, int health, int attack, int defense) {
            _grid = grid;

            MaxHealth = Health = health;
            Attack = attack;
            Defense = defense;
            Name = template.Name;
            Color = template.Color;
            Tile = template.Tile;
        }

        public Mob(GridDisplay grid, string name, int health, int attack, int defense) {
            _grid = grid;

            Name = name;
            Color = Color.White;
            Tile = '@';
            Health = MaxHealth = health;
            Attack = attack;
            Defense = defense;
        }
        
        public virtual void AttackOther(Mob other) {
            var damage = Math.Max(0, Attack - other.Defense);
            other.Health -= damage;
            LastAttack = other;
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

        public void Place(int x, int y) {
            X = x;
            Y = y;
        }

        public virtual void Draw() {
            _grid.Put(Tile, X, Y, Color);
        }
    }
}
