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
                using (var queue = new EventQueue()) {
                    while (running) {
                        while (!queue.Empty) {
                            var @event = queue.NextEvent();
                            if (@event == null) {
                                continue;
                            } else if (@event is Allegro.KeyboardEvent) {
                                Console.WriteLine("keyCode = {0}", ((Allegro.KeyboardEvent)@event).KeyCode);
                            }
                            
                        }

                        display.Clear(0, 0, 0);
                        display.Flip();
                    }
                }
            }
        }
    }
}
