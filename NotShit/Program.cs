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
            var grid = new GridDisplay(display, font);

            Console.WriteLine("grid width  = {0}", grid.GridWidth);
            Console.WriteLine("grid height = {0}", grid.GridHeight);

            Console.WriteLine("tile width  = {0}", grid.TileWidth);
            Console.WriteLine("tile height = {0}", grid.TileHeight);

            var player = new Player(grid);

            while (true) {
                while (!queue.Empty) {
                    var @event = queue.NextEvent();
                    
                    if (@event == null) {
                        continue;
                    }

                    var keyEvent = @event as Allegro.KeyboardEvent;

                    if (keyEvent != null && keyEvent.Type == Allegro.EventType.KeyChar) {
                        // always process exit
                        switch (keyEvent.KeyCode) {
                            case 17: // q
                            case 59: // ESC
                                return;
                        }

                        // mode-dependent
                        if (player.HasMessages) {
                            // only process enters
                            if (keyEvent.KeyCode == 67 || keyEvent.KeyCode == 91) {
                                player.DismissMessage();
                            }
                        } else {
                            switch (keyEvent.KeyCode) {
                                case 45: // num8
                                case 84: // up
                                    player.Move(Direction.Up);
                                    break;
                                case 39: // num2
                                case 85: // down
                                    player.Move(Direction.Down);
                                    break;
                                case 41: // num4
                                case 82: // left
                                    player.Move(Direction.Left);
                                    break;
                                case 43: // num6
                                case 83: // right
                                    player.Move(Direction.Right);
                                    break;
                                case 38: // num1
                                    player.Move(Direction.DownLeft);
                                    break;
                                case 40: // num3
                                    player.Move(Direction.DownRight);
                                    break;
                                case 44: // num7
                                    player.Move(Direction.UpLeft);
                                    break;
                                case 46:
                                    player.Move(Direction.UpRight);
                                    break;
                                case 4: // 'd' deals 3 damage
                                    player.Damage(3);
                                    break;
                            }
                        }

                        Console.WriteLine("keyCode = {0}", keyEvent.KeyCode);
                    }
                }

                // map processing
                grid.Clear();
                grid.Put('x', 0, 0);
                grid.Put('x', grid.GridWidth - 1, 0);
                grid.Put('x', 0, grid.GridHeight - 1);
                grid.Put('x', grid.GridWidth - 1, grid.GridHeight - 1);
                player.Draw();

                display.Clear(0, 0, 0);
                grid.Draw();
                // status line
                var lastY = grid.GridHeight * grid.TileHeight;
                if (player.HasMessages) {
                    font.Draw(player.CurrentMessage, 0, lastY);
                    font.Draw("[CONT]", (grid.GridWidth * grid.TileWidth) - (grid.TileWidth * "[CONT]".Length), lastY, r: 0, g: 200, b: 0);
                } else {
                    font.Draw(string.Format("HP: {0}/{1}", player.Health, player.MaxHealth), 0, lastY);
                }
                display.Flip();
            }
        }
    }
}
