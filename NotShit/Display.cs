using System;

namespace NotShit {
    class Display : IDisposable {
        private readonly IntPtr _handle;

        public Display(int width, int height) {
            _handle = Allegro.CreateDisplay(width, height);
        }

        public void Dispose() {
            Allegro.DestroyDisplay(_handle);
        }
    }
}
