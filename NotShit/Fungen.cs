using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

using NotShit.Dungen;

namespace NotShit {
    // a mob generator
    public class Fungen {
        private readonly Level _level;
        private readonly GridDisplay _grid;
        private readonly IList<MobTemplate> _templates;

        public Fungen(Level level, GridDisplay grid) {
            _level = level;
            _grid = grid;
            _templates = new List<MobTemplate>();
            
            Add('r', Color.LightGray, "rat", "1d3+1", "1d2+1", "1d4+2");
            Add('r', Color.Gray, "big rat", "2d4+2", "1d6+3", "1d6+6");
        }

        private void Add(char tile, Color color, string name, string attack, string defense, string health) {
            _templates.Add(new MobTemplate {
                Name = name,
                AttackDice = attack,
                DefenseDice = defense,
                HealthDice = health,
                Tile = tile,
                Color = color
            });
        }

        public Mob GenerateOne() {
            // completely unbiased. YOU WILL GET THE WORST MOBS ANYWAY
            var template = GenGod.SelectOne(_templates);

            var health = new Dice(template.HealthDice);
            var attack = new Dice(template.AttackDice);
            var defense = new Dice(template.DefenseDice);

            return new Mob(_level, _grid, template, health.Roll(), attack.Roll(), defense.Roll());
        }

        public void PlaceOne() {
            Point position;
            do {
                position = _level.GetWalkablePoint();
            } while (_level[position].Mob != null);

            var mob = GenerateOne();
            mob.Place(position.X, position.Y);
        }

        public Player PopulateLevel() {
            var player = new Player(_level, _grid);
            var position = _level.GetWalkablePoint();
            player.Place(position.X, position.Y);
            
            // generate mobs
            var count = GenGod.GenOne(7, 10);
            while (count-- > 0) {
                PlaceOne();
            }

            return player;
        }
    }
}