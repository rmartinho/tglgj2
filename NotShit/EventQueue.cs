using System;

namespace NotShit {
    public class EventQueue : IDisposable {
        private readonly IntPtr _handle;

        public bool Empty { get { return Allegro.IsEventQueueEmpty(_handle); } }

        public EventQueue() {
            _handle = Allegro.CreateEventQueue();
            if (_handle == IntPtr.Zero) {
                throw new Exception("No events. Ever");
            }
            Allegro.RegisterEventSource(_handle, Allegro.GetKeyboardEventSource());
        }

        public void Dispose() {
            Allegro.DestroyEventQueue(_handle);
        }

        public Allegro.IEvent NextEvent() {
            return Allegro.GetNextEvent(_handle);
        }
    }
}
