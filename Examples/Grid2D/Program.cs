using System;
using System.Collections.Generic;
using AStar;

namespace Grid2D;

class Program
{
    public static void Main(string[] args)
    {
        var grid = new Grid2D(20, 20, 25, 0, 0, 19, 19);
        var astar = new AStar.AStar(grid.Start, grid.Goal);

        var result = astar.Run();

        Console.WriteLine(result);

        var output = grid.Print(astar.GetPath());

        Console.WriteLine(output);
    }
}