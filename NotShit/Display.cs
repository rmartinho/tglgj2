using System;

namespace NotShit {
    public class Display : IDisposable {
        private readonly IntPtr _handle;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Display(string title, int width, int height) {
            _handle = Allegro.CreateDisplay(width, height);
            if (_handle == IntPtr.Zero) {
                throw new Exception("No display for you!");
            }
            Allegro.SetWindowTitle(_handle, title);

            Width = width;
            Height = height;
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

        public void Clear(Color color) {
            SetAsTarget();
            Allegro.ClearToColor(color.AllegroColor);
        }
    }
}
