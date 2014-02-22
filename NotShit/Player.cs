using System;
using System.Collections.Generic;

namespace NotShit {
    public class Player : Mob {
        private readonly Queue<string> _messages;
        private int _health;

        public override int Health {
            get { return _health; }
            set {
                if (value < 0) {
                    AddMessage("You're dead! Congratulations!");
                }
                _health = value;
            }
        }

        public bool HasMessages { get { return _messages.Count > 0; } }
        public string CurrentMessage { get { return _messages.Peek(); } }

        public Player(GridDisplay grid) : base(grid, "Player", 25, 5, 4) {
            Place(grid.GridWidth / 2, grid.GridHeight / 2);
            _messages = new Queue<string>();
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

        public override void Draw() {
            var health = (double)Health / MaxHealth;

            if (health >= 0.9) {
                Color = Color.Green;
            } else if (health >= 0.5) {
                Color = Color.Yellow;
            } else {
                Color = Color.Red;
            }

            base.Draw();
        }

        public void DebugDamage(int damage) {
            AddMessage("A mysterious force smites you!");
            Health -= damage;
        }
    }
}
