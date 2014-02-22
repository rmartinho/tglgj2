using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotShit {
    class Program {
        static void Main(string[] args) {
            Allegro.Init();
            var running = true;

            using (var display = new Display("NOT SHIT", 1280, 720)) {
                while (running) {
                    display.Clear(0, 0, 0);
                    display.Flip();
                }
            }
        }
    }
}
