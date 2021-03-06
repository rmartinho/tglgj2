﻿using System;

namespace NotShit {
    public class Font : IDisposable {
        private readonly IntPtr _handle;

        public int Ascent { get { return Allegro.GetFontAscent(_handle); } }
        public int Descent { get { return Allegro.GetFontDescent(_handle); } }
        public int LineHeight { get { return Allegro.GetFontLineHeight(_handle); } }

        public Font(string filename, int size) {
            _handle = Allegro.LoadFont(filename, size, 0);
            if (_handle == IntPtr.Zero) {
                Console.WriteLine("errno = {0}", Allegro.GetErrNo());
                throw new Exception("Font could not be loaded HAHA.");
            }
        }

        public void Dispose() {
            Allegro.DestroyFont(_handle);
        }

        public void Measure(string text, out int x, out int y, out int width, out int height) {
            Allegro.GetTextDimensions(_handle, text, out x, out y, out width, out height);
        }

        public void Draw(string text, float x, float y, Color? color = null) {
            Allegro.DrawText(_handle, (color ?? Color.White).AllegroColor, x, y, 0, text);
        }
    }
}
