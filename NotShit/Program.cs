using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotShit.Dungen;
using NotShit.Thingen;

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

            var level = new Level(grid.GridWidth, grid.GridHeight, 20, 5, 20);
            // place mobs
            var fungen = new Fungen(level, grid);
            var player = fungen.PopulateLevel();
            player.GiveThing(new Thing { Kind = ThingKind.Special });
            player.GiveThing(Thing.Random());
            player.GiveThing(Thing.Random());

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
                            // any key dismisses the current message
                            player.DismissMessage();
                            if (!player.Alive && !player.HasMessages) {
                                return;
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
                                    player.DebugDamage(3);
                                    break;
                                case 1: // 's' spawn a mob
                                    fungen.PlaceOne();
                                    break;
                            }
                        }

                        Console.WriteLine("keyCode = {0}", keyEvent.KeyCode);
                    }
                }

                // map processing
                grid.Clear();
                
                // 'ai'
                foreach (var mob in level.AliveMobs()) {
                    mob.MoveRandomly();
                }
                level.Draw(grid);

                display.Clear(Color.Black);
                grid.Draw();

                // status line
                var lastY = grid.GridHeight * grid.TileHeight;
                if (player.HasMessages) {
                    font.Draw(player.CurrentMessage, 0, lastY);
                    font.Draw("[CONT]", (grid.GridWidth * grid.TileWidth) - (grid.TileWidth * "[CONT]".Length), lastY, Color.Green);
                } else {
                    var status = string.Format("HP: {0}/{1} | Attack: {2} | Defense: {3}", player.Health, player.MaxHealth, player.Attack, player.Defense);
                    var mob = player.LastAttack;
                    if (mob != null && mob.Alive) {
                        status += string.Format(" <<>> {0} | HP: {1}/{2} | Attack: {3} | Defense: {4}", mob.Name, mob.Health, mob.MaxHealth, mob.Attack, mob.Defense);
                    }
                    font.Draw(status, 0, lastY);
                }

                display.Flip();
            }
        }
    }
}
