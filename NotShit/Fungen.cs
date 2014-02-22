using System.Collections.Generic;

namespace NotShit {
    // a mob generator
    public class Fungen {
        private readonly GridDisplay _grid;
        private readonly IList<MobTemplate> _templates;

        public Fungen(GridDisplay grid) {
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

        public Mob Generate() {
            // completely unbiased. YOU WILL GET THE WORST MOBS ANYWAY
            var idx = GenGod.GenOne(0, _templates.Count);
            var template = _templates[idx];

            var health = new Dice(template.HealthDice);
            var attack = new Dice(template.AttackDice);
            var defense = new Dice(template.DefenseDice);

            return new Mob(_grid, template, health.Roll(), attack.Roll(), defense.Roll());
        }
    }
}