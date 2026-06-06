using AStar;

namespace Grid2D;

public class Grid2D
{
    public GridNode[][] Grid;

    public int Width { get { return Grid.Length; } }
    public int Height { get { return Grid[0].Length; } }

    public GridNode Start { get; init; }
    public GridNode Goal { get; init; }

    public Grid2D(GridNode[][] grid, GridNode start, GridNode goal)
    {
        Grid = grid;
        Start = start;
        Goal = goal;
    }

    public Grid2D(int width, int height, int wallPercentage, int startX, int startY, int goalX, int goalY)
    {
        var rand = new Random();
        Start = new GridNode(this, startX, startY, false);
        Goal = new GridNode(this, goalX, goalY, false);

        Grid = new GridNode[width][];
        for (var i = 0; i < width; i++)
            Grid[i] = new GridNode[height];

        Grid[Start.X][Start.Y] = Start;
        Grid[Goal.X][Goal.Y] = Goal;

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                // don't overwrite start/goal nodes
                if (Grid[i][j] != null)
                    continue;

                Grid[i][j] = new GridNode(this, i, j, rand.Next(100) < wallPercentage);
            }
        }
    }

    public string Print(IEnumerable<INode> path)
    {
        var output = "";
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                output += Grid[i][j].Print(Start, Goal, path);
            }
            output += "\n";
        }
        return output;
    }
}
