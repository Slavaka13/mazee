using System;
using System.Collections.Generic;

class MazeGenerator
{
    private int width, height;
    private char[,] map;
    private Random rand = new Random();

    private readonly int[] dx = { 0, 0, -2, 2 };
    private readonly int[] dy = { -2, 2, 0, 0 };

    public MazeGenerator(int width, int height)
    {
        this.width = width % 2 == 0 ? width - 1 : width;
        this.height = height % 2 == 0 ? height - 1 : height;
        map = new char[this.height, this.width];
    }

    public char[,] Generate()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = '█';

        Carve(1, 1);
        return map;
    }

    private void Carve(int x, int y)
    {
        map[y, x] = ' ';

        var dirs = new List<int> { 0, 1, 2, 3 };
        Shuffle(dirs);

        foreach (var dir in dirs)
        {
            int nx = x + dx[dir];
            int ny = y + dy[dir];

            if (IsInBounds(nx, ny) && map[ny, nx] == '█')
            {
                map[y + dy[dir] / 2, x + dx[dir] / 2] = ' ';
                Carve(nx, ny);
            }
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x > 0 && y > 0 && x < width - 1 && y < height - 1;
    }
}
