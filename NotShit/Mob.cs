using System;

using NotShit.Dungen;

namespace NotShit {
    public class Mob {
        protected readonly GridDisplay _grid;
        protected readonly Level _level;

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

        public Mob(Level level, GridDisplay grid, MobTemplate template, int health, int attack, int defense) {
            _grid = grid;
            _level = level;

            MaxHealth = Health = health;
            Attack = attack;
            Defense = defense;
            Name = template.Name;
            Color = template.Color;
            Tile = template.Tile;
        }

        public Mob(Level level, GridDisplay grid, string name, int health, int attack, int defense) {
            _grid = grid;
            _level = level;

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

        public enum MoveResult {
            Attacked, Blocked, Moved
        }

        public MoveResult Move(Direction direction) {
            switch (direction) {
                case Direction.Down:
                    return Move(0, +1);
                case Direction.DownLeft:
                    return Move(-1, +1);
                case Direction.DownRight:
                    return Move(+1, +1);
                case Direction.Left:
                    return Move(-1, 0);
                case Direction.Right:
                    return Move(+1, 0);
                case Direction.Up:
                    return Move(0, -1);
                case Direction.UpLeft:
                    return Move(-1, -1);
                case Direction.UpRight:
                    return Move(+1, -1);
            }
            return MoveResult.Blocked;
        }

        private MoveResult Move(int dx, int dy) {
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

            var oldTile = _level[new Point(X, Y)];
            var newTile = _level[new Point(newX, newY)];

            if (newTile.Kind == TileKind.Wall) {
                return MoveResult.Blocked;
            }

            if (newTile.Mob != null) {
                AttackOther(newTile.Mob);
                return MoveResult.Attacked;
            }

            oldTile.Mob = null;
            newTile.Mob = this;
            
            X = newX;
            Y = newY;
            return MoveResult.Moved;
        }

        public void Place(int newX, int newY) {
            X = newX;
            Y = newY;
            var tile = _level[new Point(X, Y)];
            tile.Mob = this;
        }

        public virtual void Draw() {
            _grid.Put(Tile, X, Y, Color);
        }

        public void MoveRandomly() {
            var directions = new[] {
                Direction.Up,
                Direction.Down,
                Direction.Left,
                Direction.Right,
                Direction.UpRight,
                Direction.UpLeft,
                Direction.DownLeft,
                Direction.DownRight
            };

            Direction direction;
            do {
                direction = GenGod.SelectOne(directions);
            } while (Move(direction) == MoveResult.Blocked);
        }
    }
}
