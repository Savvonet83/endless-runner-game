using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CSharpEndlessRunner
{
    class Obstacle { public int X; public int Y; public bool IsFuel; }

    class Program
    {
        static int Width = 40;
        static int Height = 20;
        static int PlayerX = Width / 2;
        static int PlayerY = Height - 2;
        static List<Obstacle> obstacles = new();
        static Random rng = new();
        static double fuel = 100.0;
        static double distance = 0.0;
        static bool running = true;

        static void Main()
        {
            Console.CancelKeyPress += (_, e) => { running = false; e.Cancel = true; };
            try { Console.CursorVisible = false; } catch { }

            var sw = Stopwatch.StartNew();
            double lastSpawn = 0;
            double last = sw.Elapsed.TotalSeconds;

            while (running)
            {
                double now = sw.Elapsed.TotalSeconds;
                double dt = Math.Min(0.1, now - last);
                last = now;

                HandleInput();
                // spawn
                if (now - lastSpawn > Math.Max(0.3, 1.5 - distance/200))
                {
                    Spawn();
                    lastSpawn = now;
                }

                Update((float)dt);
                Render();

                if (fuel <= 0)
                {
                    running = false;
                }

                Thread.Sleep(70);
            }

            Console.SetCursorPosition(0, Height + 2);
            Console.CursorVisible = true;
            Console.WriteLine();
            Console.WriteLine($"Game Over — Distanza percorsa: {Math.Floor(distance)}  |  Fuel rimasto: {Math.Floor(fuel)}");
            Console.WriteLine("Premi un tasto per uscire...");
            Console.ReadKey(true);
        }

        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                var k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.LeftArrow || k == ConsoleKey.A) PlayerX = Math.Max(1, PlayerX - 1);
                if (k == ConsoleKey.RightArrow || k == ConsoleKey.D) PlayerX = Math.Min(Width - 2, PlayerX + 1);
                if (k == ConsoleKey.Escape) running = false;
            }
        }

        static void Spawn()
        {
            int x = rng.Next(1, Width - 1);
            bool isFuel = rng.NextDouble() < Math.Max(0.06, 0.12 - distance/1000);
            obstacles.Add(new Obstacle { X = x, Y = 0, IsFuel = isFuel });
        }

        static void Update(float dt)
        {
            // move obstacles
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                obstacles[i].Y += 1;
                if (obstacles[i].Y > Height) obstacles.RemoveAt(i);
            }

            // collisions
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                var o = obstacles[i];
                if (o.Y == PlayerY && o.X == PlayerX)
                {
                    if (o.IsFuel)
                    {
                        fuel = Math.Min(200, fuel + 30);
                    }
                    else
                    {
                        // hit traffic: reduce fuel and distance penalty
                        fuel -= 20;
                        distance = Math.Max(0, distance - 10);
                    }
                    obstacles.RemoveAt(i);
                }
            }

            // fuel consumption and distance
            fuel -= 6 * dt; // consumption
            distance += 30 * dt; // rates -> adjust for speed feel
        }

        static void Render()
        {
            var sb = new StringBuilder();
            // top status
            sb.AppendLine($"Distanza: {Math.Floor(distance)} m    Fuel: {Math.Floor(fuel)}");
            // playfield
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    char ch = ' ';
                    if (x == 0 || x == Width - 1) ch = '|';
                    // obstacles
                    foreach (var o in obstacles)
                    {
                        if (o.X == x && o.Y == y)
                        {
                            ch = o.IsFuel ? 'F' : 'X';
                            break;
                        }
                    }
                    // player
                    if (x == PlayerX && y == PlayerY) ch = 'A';
                    sb.Append(ch);
                }
                sb.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }
    }
}
