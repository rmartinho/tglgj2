using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace NotShit {
    // a mob generator
    public class Fungen {
        private readonly IList<MobTemplate> _templates;

        public Fungen() {
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
    }
}