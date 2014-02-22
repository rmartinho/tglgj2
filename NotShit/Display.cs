using System;

namespace NotShit {
    class Display : IDisposable {
        private readonly IntPtr _handle;

        public Display(string title, int width, int height) {
            _handle = Allegro.CreateDisplay(width, height);
            Allegro.SetWindowTitle(_handle, title);
        }

        public void Dispose() {
            Allegro.DestroyDisplay(_handle);
        }

        public void Flip() {
            Allegro.FlipDisplay();
        }

        public void Clear(byte r, byte g, byte b) {
            Allegro.SetTargetBackbuffer(_handle);
            Allegro.ClearToColor(Allegro.MapRGB(r, g, b));
        }
    }
}
