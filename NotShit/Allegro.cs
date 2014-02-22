using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NotShit {
    public static class Allegro {
        private const string AllegroDll = "lib/allegro-5.0.10-monolith-mt.dll";
        
        /**
         * This is a stupid #define that bitshifts a lot of things.
         * Calculated manually because fuck using C++/CLI.
         */
        private const int AllegroVersionInt = 83888641;

        [DllImport(AllegroDll, EntryPoint = "al_install_system", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InstallSystem(int version, IntPtr atExit);

        [DllImport(AllegroDll, EntryPoint = "al_init_ttf_addon", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InitTTF();

        [DllImport(AllegroDll, EntryPoint = "al_init_font_addon", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InitFont();

        [DllImport(AllegroDll, EntryPoint = "al_init_image_addon", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InitImage();

        [DllImport(AllegroDll, EntryPoint = "al_install_keyboard", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InstallKeyboard();

        [DllImport(AllegroDll, EntryPoint = "al_install_mouse", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InstallMouse();

        [DllImport(AllegroDll, EntryPoint = "al_create_display", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateDisplay(int width, int height);

        [DllImport(AllegroDll, EntryPoint = "al_destroy_display", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyDisplay(IntPtr handle);

        [DllImport(AllegroDll, EntryPoint = "al_flip_display", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FlipDisplay();

        [DllImport(AllegroDll, EntryPoint = "al_set_window_title", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowTitle(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string title);

        [DllImport(AllegroDll, EntryPoint = "al_map_rgb", CallingConvention = CallingConvention.Cdecl)]
        public static extern Color MapRGB(byte r, byte g, byte b);

        [DllImport(AllegroDll, EntryPoint = "al_map_rgb_f", CallingConvention = CallingConvention.Cdecl)]
        public static extern Color MapRGB(float r, float g, float b);

        [DllImport(AllegroDll, EntryPoint = "al_map_rgba", CallingConvention = CallingConvention.Cdecl)]
        public static extern Color MapRGB(byte r, byte g, byte b, byte a);

        [DllImport(AllegroDll, EntryPoint = "al_map_rgba_f", CallingConvention = CallingConvention.Cdecl)]
        public static extern Color MapRGB(float r, float g, float b, float a);

        [DllImport(AllegroDll, EntryPoint = "al_clear_to_color", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearToColor(Color color);

        [DllImport(AllegroDll, EntryPoint = "al_set_target_bitmap", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTargetBitmap(IntPtr handle);

        [DllImport(AllegroDll, EntryPoint = "al_set_target_backbuffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTargetBackbuffer(IntPtr handle);

        [DllImport(AllegroDll, EntryPoint = "al_create_event_queue", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateEventQueue();

        [DllImport(AllegroDll, EntryPoint = "al_destroy_event_queue", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyEventQueue(IntPtr handle);

        [DllImport(AllegroDll, EntryPoint = "al_register_event_source", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RegisterEventSource(IntPtr queueHandle, IntPtr sourceHandle);

        [DllImport(AllegroDll, EntryPoint = "al_is_event_queue_empty", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsEventQueueEmpty(IntPtr handle);

        [DllImport(AllegroDll, EntryPoint = "al_get_next_event", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetNextEventImpl(IntPtr handle, out EventUnion @event);

        [DllImport(AllegroDll, EntryPoint = "al_get_display_event_source", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetDisplayEventSource();

        [DllImport(AllegroDll, EntryPoint = "al_get_keyboard_event_source", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetKeyboardEventSource();

        [DllImport(AllegroDll, EntryPoint = "al_get_mouse_event_source", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetMouseEventSource();

        [StructLayout(LayoutKind.Sequential)]
        public struct Color {
            public float r, g, b, a;
        }

        public enum EventType {
            KeyDown               = 10,
            KeyChar               = 11,
            KeyUp                 = 12,
            MouseAxes             = 20,
            MouseButtonDown       = 21,
            MouseButtonUp         = 22,
            MouseEnterDisplay     = 23,
            MouseLeaveDisplay     = 24,
            MouseWarped           = 25,
            Timer                 = 30,
            DisplayExpose         = 40,
            DisplayResize         = 41,
            DisplayClose          = 42,
            DisplayLost           = 43,
            DisplayFound          = 44,
            DisplaySwitchIn       = 45,
            DisplaySwitchOut      = 46,
            DisplayOrientation    = 47
        }

        /**
         * THIS IS FUN. YAY UNIONS.
         * (Joystick is not included)
         * 
         * I sure hope there is no padding!
         */
        [StructLayout(LayoutKind.Explicit)]
        public struct EventUnion {
            // Common to all
            [FieldOffset(0)]
            public EventType type;

            [FieldOffset(4)]
            public IntPtr source;

            [FieldOffset(8)]
            public double timestamp;

            // Common to keyboard/mouse
            [FieldOffset(16)]
            public IntPtr display;

            // ALLEGRO_KEYBOARD_EVENT
            [FieldOffset(20)]
            public int keyCode;

            [FieldOffset(24)]
            public int uniChar;

            [FieldOffset(28)]
            public uint modifiers;

            [FieldOffset(32)]
            public int repeat; // actually bool, but C

            // ALLEGRO_MOUSE_EVENT
            [FieldOffset(20)]
            public int x;

            [FieldOffset(24)]
            public int y;

            [FieldOffset(28)]
            public int z;

            [FieldOffset(32)]
            public int w;

            [FieldOffset(36)]
            public int dx;

            [FieldOffset(40)]
            public int dy;

            [FieldOffset(44)]
            public int dz;

            [FieldOffset(48)]
            public int dw;

            [FieldOffset(52)]
            public uint button;

            [FieldOffset(56)]
            public float pressure;

            // ALLEGRO_DISPLAY_EVENT (this doesn't have `display`, because display is the source)
            [FieldOffset(16)]
            public int displayX;

            [FieldOffset(20)]
            public int displayY;

            [FieldOffset(24)]
            public int width;

            [FieldOffset(28)]
            public int height;

            [FieldOffset(32)]
            public int orientation;

            // ALLEGRO_TIMER_EVENT
            [FieldOffset(16)]
            public long count;

            [FieldOffset(24)]
            public double error;
        }

        public interface IEvent {
        }

        public class KeyboardEvent : IEvent {
            public IntPtr DisplayHandle { get; set; }
            public int KeyCode { get; set; }
            public int Character { get; set; }
            public uint Modifiers { get; set; }
            public bool Repeat { get; set; }

            internal KeyboardEvent(EventUnion union) {
                var types = new[] {
                    EventType.KeyChar,
                    EventType.KeyDown,
                    EventType.KeyUp
                };
                
                Debug.Assert(types.Contains(union.type), "Bad union tag");

                DisplayHandle = union.display;
                KeyCode = union.keyCode;
                Character = union.uniChar;
                Modifiers = union.modifiers;
                Repeat = union.repeat != 0;
            }
        }

        public static void Init() {
            if (!InstallSystem(AllegroVersionInt, IntPtr.Zero)) {
                throw new Exception("Allegro doesn't like you.");
            }

            if (!InitTTF()) {
                throw new Exception("TTF addon failed to init.");
            }

            if (!InitFont()) {
                throw new Exception("Font addon failed to init.");
            }

            if (!InitImage()) {
                throw new Exception("Image addon failed to init.");
            }

            if (!InstallKeyboard()) {
                throw new Exception("Keyboard failed to install. How that happens I have no idea.");
            }

            if (!InstallMouse()) {
                throw new Exception("Your OS is broken or something. Mouse failed to install.");
            }
        }

        public static IEvent GetNextEvent(IntPtr handle) {
            EventUnion union;
            
            if (!GetNextEventImpl(handle, out union)) {
                throw new Exception("Failed to fetch an event");
            }

            switch (union.type) {
                case EventType.KeyChar:
                case EventType.KeyDown:
                case EventType.KeyUp:
                    return new KeyboardEvent(union);
                default:
                    return null;
            }
        }
    }
}
