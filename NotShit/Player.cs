﻿using System;
using System.Collections.Generic;
using NotShit.Thingen;

using NotShit.Dungen;

namespace NotShit {
    public class Player : Mob {
        private readonly Queue<string> _messages;

        private readonly List<Thing> things = new List<Thing>();

        public override int Health {
            get { return base.Health; }
            set {
                if (value < 0) {
                    AddMessage("You're dead! Congratulations!");
                }
                base.Health = value;
            }
        }

        public void GiveThing(Thing thing)
        {
            AddMessage("You obtained a " + thing);
            if (thing.Kind == ThingKind.Weapon)
            {
                Attack += thing.Bonus;
            }
            if (thing.Kind == ThingKind.Armor)
            {
                Defense += thing.Bonus;
            }
        }

        public bool HasMessages { get { return _messages.Count > 0; } }
        public string CurrentMessage { get { return _messages.Peek(); } }

        public Player(Level level, GridDisplay grid) : base(level, grid, "Player", 25, 5, 4) {
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

        public override void AttackOther(Mob other) {
            base.AttackOther(other);
            if (other.Health < 0) {
                _level[new Point(other.X, other.Y)].Mob = null;
                AddMessage(string.Format("You kill the {0}!", other.Name));
            }
        }
    }
}
