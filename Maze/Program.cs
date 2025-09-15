using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        Maze maze = new Maze(71, 41); // ширина 81, высота 41


        int playerX = maze.StartX;
        int playerY = maze.StartY;

        maze.Draw(playerX, playerY);

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            int newX = playerX;
            int newY = playerY;

            switch (key)
            {
                case ConsoleKey.UpArrow: newY--; break;
                case ConsoleKey.DownArrow: newY++; break;
                case ConsoleKey.LeftArrow: newX--; break;
                case ConsoleKey.RightArrow: newX++; break;
                case ConsoleKey.P:
                    maze.ShowPath(playerX, playerY);
                    maze.Draw(playerX, playerY);
                    continue;
            }

            if (maze.IsWalkable(newX, newY))
            {
                playerX = newX;
                playerY = newY;
                maze.ClearPath();
                maze.Draw(playerX, playerY);
            }

            if (maze.IsExit(playerX, playerY))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("🎉 Вы выбрались из лабиринта!");
                Console.ResetColor();
                break;
            }
        }
    }
}
