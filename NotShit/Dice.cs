using System;
using System.Text.RegularExpressions;

namespace NotShit {
    public class Dice {
        private readonly static Regex DiceRegex = new Regex(@"^([0-9]+?)?d([0-9]+?)([+-][0-9]+?)?$");

        public int Sides { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }

        public Dice(string designator) {
            var match = DiceRegex.Match(designator);
            var count = match.Groups[1];
            var sides = match.Groups[2];
            var offset = match.Groups[3];

            Sides = Convert.ToInt32(sides.ToString());

            try {
                Count = Convert.ToInt32(count.ToString());
            } catch {
                Count = 1;
            }

            try {
                Offset = Convert.ToInt32(offset.ToString());
            } catch {
                Offset = 0;
            }
        }

        public Dice() {
            Sides = 1;
            Count = 1;
            Offset = 0;
        }

        public int Roll() {
            return GenGod.RollDice(Count, Sides) + Offset;
        }
    }
}
