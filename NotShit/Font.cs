using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public void Draw(string text, float x, float y, byte r = 255, byte g = 255, byte b = 255) {
            var color = Allegro.MapRGB(r, g, b);
            Allegro.DrawText(_handle, color, x, y, 0, text);
        }
    }
}
