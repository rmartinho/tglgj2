using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotShit {
    class Program {
        static void Main(string[] args) {
            Allegro.Init();
            using (var display = new Display(1280, 720)) {
                Console.Write("hello");
            }
        }
    }
}
