using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotShit {
    class Program {
        static void Main(string[] args) {
            Allegro.Init();
            
            using (var display = new Display("NOT SHIT", 1280, 720)) {
                using (var queue = new EventQueue()) {
                    using (var font = new Font("fonts/DejaVuSansMono.ttf", 16)) {
                        Main(display, queue, font);
                    }
                }
            }
        }

        static void Main(Display display, EventQueue queue, Font font) {
            var running = true;
            
            while (running) {
                while (!queue.Empty) {
                    var @event = queue.NextEvent();
                    
                    if (@event == null) {
                        continue;
                    }

                    var keyEvent = @event as Allegro.KeyboardEvent;

                    if (keyEvent != null && keyEvent.Type == Allegro.EventType.KeyDown) {
                        if (keyEvent.KeyCode == 17 || keyEvent.KeyCode == 59) { // 'q' or escape
                            running = false;
                        }

                        Console.WriteLine("keyCode = {0}", keyEvent.KeyCode);
                    }
                }

                display.Clear(0, 0, 0);
                font.Draw("test", 0, 0);
                display.Flip();
            }
        }
    }
}
