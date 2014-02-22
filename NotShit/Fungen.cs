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

        public Player PopulateLevel() {
            var player = new Player(_level, _grid);
            var room = GenGod.SelectOne(_level.Rooms);
            var position = room.TopLeft + new Point(1, 1);
            player.Place(position.X, position.Y);

            // generate mobs
            var count = GenGod.GenOne(7, 10);
            while (count > 0) {
                var iters = 0;
                do {
                    room = GenGod.SelectOne(_level.Rooms);
                    var startX = room.TopLeft.X + 1;
                    var startY = room.TopLeft.Y + 1;
                    var endX = room.BottomRight.X - 1;
                    var endY = room.BottomRight.Y - 1;
                    if (startX > endX) {
                        
                    }
                    if (endX > endY) {
                        
                    }
                    position = GenGod.Point(startX, startY, endX, endY);
                    if (iters++ > 20) {
                        // bruteforce ahoy (I don't want infinite loops though)
                        count--;
                        position.X = position.Y = -1;
                        break;
                    }
                } while (_level[position].Kind == TileKind.Wall || _level[position].Mob != null);

                if (position.X != -1) {
                    var mob = GenerateOne();
                    mob.Place(position.X, position.Y);
                }
            }

            return player;
        }

        public void DebugSpawn(Player player) {
            var mob = GenerateOne();
            // find a tile that's not a wall
            //_level[new Point(player.X, player.Y)]
        }
    }
}