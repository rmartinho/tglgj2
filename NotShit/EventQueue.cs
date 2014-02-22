using System;

namespace NotShit {
    public class EventQueue : IDisposable {
        private IntPtr _handle;

        public bool Empty { get { return Allegro.IsEventQueueEmpty(_handle); } }

        public EventQueue() {
            _handle = Allegro.CreateEventQueue();
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
