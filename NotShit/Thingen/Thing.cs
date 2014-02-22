namespace NotShit.Thingen
{
    public enum ThingKind
    {
        Weapon, Armor, Special,
    }

    public class Thing
    {
        public static Thing Random()
        {
            return new Thing
            {
                Kind = GenGod.GenOne(0, 2) == 0 ? ThingKind.Weapon : ThingKind.Armor,
                Bonus = GenGod.GenOne(0, 10),
                Identified = true
            };
        }

        public int Bonus { get; set; }
        public ThingKind Kind { get; set; }

        public string UnidentifiedDescription
        {
            get {
                switch (Kind)
                {
                    case ThingKind.Armor:
                        return "a stab avoidance device";
                    case ThingKind.Weapon:
                        return "a stabbing device";
                    case ThingKind.Special:
                        return "a zombie robot that needs repairing";
                }
                return "a WTF";
            }
        }

        public string IdentifiedDescription
        {
            get
            {
                switch (Kind)
                {
                    case ThingKind.Armor:
                        return "+" + Bonus + " armor of protection";
                    case ThingKind.Weapon:
                        return "+" + Bonus + " sword of destruction";
                    case ThingKind.Special:
                        return "an identified zombie robot that needs repairing";
                }
                return "an identified WTF. WTF?";
            }
        }

        public bool Identified { get; set; }

        public string Description
        {
            get { return Identified ? IdentifiedDescription : UnidentifiedDescription; }
        }

        public override string ToString() // Shit, forgot about my tea
        {
            return IdentifiedDescription;
        }
    }
}