using System;
using System.Linq;

namespace NotShit
{
    public static class GenGod
    {
        private static Random generator = new Random();

        public static void Reseed(int seed)
        {
            generator = new Random(seed);
        }

        public static int GenOne(int lowerInclusive, int upperExclusive)
        {
            return generator.Next(lowerInclusive, upperExclusive);
        }

        public static int RollDice(int number, int sides)
        {
            return Enumerable.Range(0, number).Sum(_ => RollDie(sides));
        }

        public static int RollDie(int sides)
        {
            return generator.Next(0, sides) + 1;
        }
    }
}