using System;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace NotShit {
    static class Allegro {
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
    }
}
