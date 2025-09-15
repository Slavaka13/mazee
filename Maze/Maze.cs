using System;
using System.Collections.Generic;

class Maze
{
    public char[,] Map { get; private set; }
    public int Width => Map.GetLength(1);
    public int Height => Map.GetLength(0);
    public int StartX { get; private set; }
    public int StartY { get; private set; }
    public int ExitX { get; private set; }
    public int ExitY { get; private set; }

    private List<(int x, int y)> currentPath = null;

    public Maze(int width, int height)
    {
        MazeGenerator generator = new MazeGenerator(width, height);
        Map = generator.Generate();

        StartX = 1;
        StartY = 1;
        ExitX = width - 2;
        ExitY = height - 2;

        // Гарантируем доступность выхода
        Map[ExitY, ExitX] = ' ';
        Map[ExitY, ExitX] = 'E';
    }

    public void Draw(int playerX, int playerY)
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                bool isPath = currentPath?.Contains((x, y)) ?? false;

                if (x == playerX && y == playerY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("@ ");
                }
                else if (isPath)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("· ");
                }
                else
                {
                    char c = Map[y, x];

                    if (c == '█')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("██");
                    }
                    else if (c == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("E ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
            }
            Console.WriteLine();
        }

        Console.ResetColor();
        Console.WriteLine("Стрелки — движение, P — показать путь до выхода");
    }

    public void ShowPath(int fromX, int fromY)
    {
        currentPath = FindShortestPath(fromX, fromY, ExitX, ExitY);
        if (currentPath == null)
        {
            Console.SetCursorPosition(0, Height + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Путь не найден!");
            Console.ResetColor();
        }
    }

    public void ClearPath()
    {
        currentPath = null;
    }

    private List<(int x, int y)> FindShortestPath(int startX, int startY, int endX, int endY)
    {
        var visited = new bool[Height, Width];
        var prev = new (int x, int y)?[Height, Width];

        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((startX, startY));
        visited[startY, startX] = true;

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (x == endX && y == endY)
            {
                var path = new List<(int x, int y)>();
                var current = (x, y);

                while (current != (startX, startY))
                {
                    path.Add(current);
                    current = prev[current.y, current.x].Value;
                }

                path.Reverse();
                return path;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (IsWalkable(nx, ny) && !visited[ny, nx])
                {
                    queue.Enqueue((nx, ny));
                    visited[ny, nx] = true;
                    prev[ny, nx] = (x, y);
                }
            }
        }

        return null;
    }

    public bool IsWalkable(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height &&
               (Map[y, x] == ' ' || Map[y, x] == 'E');
    }

    public bool IsExit(int x, int y)
    {
        return x == ExitX && y == ExitY;
    }
}
