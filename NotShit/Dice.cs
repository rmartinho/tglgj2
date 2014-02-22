namespace NotShit {
    public class Dice {
        public int Sides { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }

        public int Roll() {
            return GenGod.RollDice(Count, Sides) + Offset;
        }
    }
}
