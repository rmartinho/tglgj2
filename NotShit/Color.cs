namespace NotShit {
    public struct Color {
        public static Color Red = new Color(red: 255);
        public static Color Green = new Color(green: 255);
        public static Color Blue = new Color(blue: 255);
        public static Color White = new Color(255, 255, 255);
        public static Color Black = new Color();
        public static Color Gray = new Color(128, 128, 128);
        public static Color LightGray = new Color(200, 200, 200);
        public static Color Yellow = new Color(red: 255, green: 255);

        public Allegro.Color AllegroColor;

        public Color(byte red = 0, byte green = 0, byte blue = 0) {
            AllegroColor = Allegro.MapRGB(red, green, blue);
        }
    }
}
