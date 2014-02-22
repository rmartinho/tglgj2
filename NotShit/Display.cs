using System;

namespace NotShit {
    class Display : IDisposable {
        private readonly IntPtr _handle;

        public Display(string title, int width, int height) {
            _handle = Allegro.CreateDisplay(width, height);
            if (_handle == IntPtr.Zero) {
                throw new Exception("No display for you!");
            }
            Allegro.SetWindowTitle(_handle, title);
        }

        public void Dispose() {
            Allegro.DestroyDisplay(_handle);
        }

        public void Flip() {
            Allegro.FlipDisplay();
        }

        public void SetAsTarget() {
            Allegro.SetTargetBackbuffer(_handle);
        }

        public void Clear(byte r, byte g, byte b) {
            SetAsTarget();
            Allegro.ClearToColor(Allegro.MapRGB(r, g, b));
        }
    }
}
