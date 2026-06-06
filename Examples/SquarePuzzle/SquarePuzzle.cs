using System;
using System.Collections.Generic;
using System.Data;
using AStar;

namespace SquarePuzzle;

/// <summary>
/// The implementation of the AStarLib.INode.
/// This implementation of A* does not create a graph before running.  It generates
/// children nodes when INode.Children is called.  Each child moves the 'space' character
/// in the SquarePuzzle in all valid moves.  This continues until the 'goal' node or state
/// is reached.
/// </summary>
public class SquarePuzzle : INode
{
    /// <summary>
    /// True if this node has already been searched.
    /// </summary>
    private bool isClosedList;

    private bool isOpenList;
    
    /// <summary>
    /// The grid of Nodes for this current state of the square puzzle.
    /// </summary>
    public required SquareNode[][] Grid;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="openList"></param>
    /// <returns></returns>
    public bool IsOpenList(IEnumerable<INode> openList)
    {
        foreach (var node in openList)
        {
            if (Equal(node))
            {
                return true;
            }
        }
        return false;
    }

    public void SetOpenList(bool value)
    {
        isOpenList = value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="closedList"></param>
    /// <returns></returns>
    public bool IsClosedList(IEnumerable<INode> closedList)
    {
        if (isClosedList)
            return true;

        foreach (var node in closedList)
        {
            if (Equal(node))
            {
                return true;
            }
        }
        return false;

        //get
        //{
        //    // If no parent, or no parent.parent then it is not on the closed list, otherwise make sure you are not backtracking on your moves.
        //    // Backtracking is equal to this nodes Parent.Parent since that is two moves previous, or in other words the same exact state for the grid.
        //    return (Parent == null || Parent.Parent == null) ? false : Equal(Parent.Parent);
        //}
        //set { isClosedList = value; }
    }

    public void SetClosedList(bool value)
    {
        isClosedList = value;
    }
    
    /// <summary>
    /// Gets the total cost, or f, for this square puzzle state.
    /// </summary>
    public int TotalCost { get { return MovementCost + EstimatedCost; } }
    
    /// <summary>
    /// Gets or sets the movement cost, or g, for this square puzzle state.
    /// </summary>
    public int MovementCost { get; set; }
    
    /// <summary>
    /// Gets or sets the estimated cost, or h, for this square puzzle state.
    /// </summary>
    public int EstimatedCost { get; set; }
    
    /// <summary>
    /// Sets the movement cost, or g, for this square puzzle state.
    /// </summary>
    /// <param name="parent">Access to this square puzzles state's parent for its movement cost.</param>
    public void SetMovementCost(INode parent)
    {
        MovementCost = parent.MovementCost + 1;
    }
    
    /// <summary>
    /// Sets the estimated cost, or h, for this square puzzle state.
    /// This heuristic uses a modified version of Nilsson's sequence score.
    /// If the "space" node is not in the correct position, then add 1.
    /// Check each node and if its neighbors are not correct, then score two per non-corect neighbor.
    /// Multiply this by 3.
    /// Add the manhatten distance for each node, except the "space" node to the total.
    /// </summary>
    /// <param name="goal">Access to the square puzzle state that is the goal for calculating the heuristic.</param>
    public void SetEstimatedCost(INode goal)
    {
        SquarePuzzle g = (SquarePuzzle)goal;
        int[] pos = {
                        1, -1, 0, 0,    //x
                        0, 0, 1, -1     //y
                    };
        int[] plus = { 1, -1, 3, -3 };
        // Uses Nilsson's sequence score.
        for (int i = 0; i < Grid.Length; i++)
        {
            for (int j = 0; j < Grid[i].Length; j++)
            {
                SquareNode node = Grid[i][j];
                //Any nodes next to it that are in the "correct" spot get a +2
                List<SquareNode> neighbors = Neighbors(node);
                for (int k = 0; k < neighbors.Count; k++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        //todo should only check 4x1 instead of 4x4.
                        if (neighbors[k].Position.X == node.Position.X + pos[z] &&
                            neighbors[k].Position.Y == node.Position.Y + pos[z + 4])
                            if (neighbors[k].Number != node.Number + plus[z])
                                EstimatedCost += 2;
                    }
                }
            }
        }
        //If the goal "space" node is not "space" then add 1.
        if (!Grid[Grid.Length - 1][Grid.Length - 1].IsSpace)
        {
            EstimatedCost += 1;
        }

        EstimatedCost *= 3;

        for (int i = 0; i < Grid.Length; i++)
        {
            for (int j = 0; j < Grid[i].Length; j++)
            {
                var node = Grid[i][j];
                var goalNode = GetNodeByNumber(g, node.Number);
                if (goalNode == null)
                {
                    throw new InvalidConstraintException("Goal node was not found but should always be set.");
                }
                
                //If the node isn't where it should be then add its manhattan distance to its goal position.
                if (!node.IsSpace &&
                    (goalNode.Position.X != node.Position.X ||
                        goalNode.Position.Y != node.Position.Y))
                    EstimatedCost += Math.Abs(node.Position.X - goalNode.Position.X) +
                                        Math.Abs(node.Position.Y - goalNode.Position.Y);
            }
        }
    }
    
    /// <summary>
    /// Gets or sets the parent square puzzle state for this square puzzle state.
    /// </summary>
    public INode? Parent { get; set; }

    /// <summary>
    /// Gets the space node of this square puzzle.
    /// </summary>
    /// <exception cref="NoSpaceException">Thrown when there is no space in the grid.</exception>
    /// <returns>The space <see cref="SquareNode"/>.</returns>
    public SquareNode GetSpace()
    {
        for (var i = 0; i < Grid.Length; i++)
        {
            for (var j = 0; j < Grid[i].Length; j++)
            {
                if (Grid[i][j].IsSpace)
                {
                    return Grid[i][j];
                }
            }
        }

        throw new NoSpaceException(this);
    }

    /// <summary>
    /// Returns all of the children for this square puzzle state.  Generates them when called.
    /// Children in the AStar implementation of the square puzzle are all possible moves for 
    /// the next move of the "space" square.
    /// </summary>
    /// <returns>Returns all of the children for this square puzzle state.</returns>
    public IEnumerable<INode> Children
    {
        get
        {
            var children = new List<INode>();
            var space = GetSpace();

            var neighbors = Neighbors(space);
            for (var i = 0; i < neighbors.Count; i++)
            {
                children.Add(Copy(this));
                Swap(((SquarePuzzle)children[i]).Grid, space.Position, neighbors[i].Position);
            }
            return children;
        }
    }
    
    /// <summary>
    /// Returns whether this square puzzle state is equal to the goal square puzzle state.
    /// </summary>
    /// <param name="goal">The goal square puzzle state.</param>
    /// <returns>Returns whether this square puzzle state is equal to the goal square puzzle state.</returns>
    public bool IsGoal(INode goal)
    {
        return Equal(goal);
    }
    
    /// <summary>
    /// Returns whether this square puzzle state is equal to the parameter square puzzle state.
    /// </summary>
    /// <param name="state">State to compare for equality.</param>
    /// <returns>Returns whether this square puzzle state is equal to the parameter square puzzle state.</returns>
    public bool Equal(INode state)
    {
        SquarePuzzle s = (SquarePuzzle)state;
        for (int i = 0; i < s.Grid.Length; i++)
        {
            for (int j = 0; j < s.Grid[i].Length; j++)
            {
                if (s.Grid[i][j].Number != Grid[i][j].Number)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    /// <summary>
    /// Prints off the current square puzzle state to Console.Out.
    /// </summary>
    public void Print()
    {
        for (int i = 0; i < Grid.Length; i++)
        {
            for (int j = 0; j < Grid.Length; j++)
            {
                Console.Out.Write(Grid[i][j].ToString());
            }
            Console.Out.Write("\n");
        }
        Console.Out.Write("\n");
    }
    
    /// <summary>
    /// Returns up to all four neighbors of the node that is passed.
    /// </summary>
    /// <param name="node">Node to get its neighbors.</param>
    /// <returns>Returns up to all four neighbors of the node that is passed.</returns>
    public List<SquareNode> Neighbors(SquareNode node)
    {
        int[] n =    { 1, -1, 0, 0,     //x
                      0, 0, 1, -1 };    //y
        var p = node.Position;
        var neighbors = new List<SquareNode>();
        for (int i = 0; i < 4; i++)
        {
            int x = p.X + n[i];
            int y = p.Y + n[i + 4];
            if (x >= 0 &&
                x < Grid.Length &&
                y >= 0 &&
                y < Grid.Length)
                neighbors.Add(Grid[x][y]);
        }
        return neighbors;
    }
    
    /// <summary>
    /// Swaps the two nodes in the grid provided.
    /// Is used when generating children.
    /// Copy the parent and swap for the next move.
    /// </summary>
    private static void Swap(SquareNode[][] grid, Point p1, Point p2)
    {
        grid[p1.X][p1.Y].Position = p2;
        grid[p2.X][p2.Y].Position = p1;
        (grid[p2.X][p2.Y], grid[p1.X][p1.Y]) = (grid[p1.X][p1.Y], grid[p2.X][p2.Y]);
    }
    
    /// <summary>
    /// Gets a node in the square puzzle by its number.
    /// Returns null if no node has that number.
    /// </summary>
    /// <param name="p">Puzzle to search in.</param>
    /// <param name="num">The node with this number to get.</param>
    /// <returns>Returns the node in the square puzzle with the specified number.</returns>
    public static SquareNode? GetNodeByNumber(SquarePuzzle p, int num)
    {
        for (int i = 0; i < p.Grid.Length; i++)
        {
            for (int j = 0; j < p.Grid.Length; j++)
            {
                if (p.Grid[i][j].Number == num)
                {
                    return p.Grid[i][j];
                }
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Creates a square puzzle that starts at 1, and finishes at size * size.
    /// So if the size = 3 then it will produce 3 * 3 = 9:
    /// 1 2 3
    /// 4 5 6
    /// 7 8 9
    /// The highest number is the "space" square.
    /// This is good for generating a linear "goal state" square puzzle.
    /// </summary>
    /// <param name="size">Size of the square puzzle. eg size * size.  The size must be >= 2.</param>
    /// <returns>Returns a linear square puzzle.</returns>
    public static SquarePuzzle CreateLinear(int size)
    {
        var goal = new SquarePuzzle
        {
            Grid = new SquareNode[size][],
        };
        for (var i = 0; i < size; i++)
        {
            goal.Grid[i] = new SquareNode[size];
        }
        var number = 1;
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                goal.Grid[i][j] = new SquareNode(new Point(i, j), number++);
            }
        }

        //for (int i = 0; i < size; i++)
        //    for (int j = 0; j < size; j++)
        //        goal.Grid[i][j].Initialize(new Node.Point(i, j), new Node.Point(i, j), number++);
        goal.Grid[goal.Grid.Length - 1][goal.Grid.Length - 1].IsSpace = true;
        return goal;
    }

    public static void Shuffle(SquarePuzzle puzzle, int iterations)
    {
        for(var i = 0; i < iterations; i++)
        {
            var children = (List<INode>)puzzle.Children;
            children.Shuffle();
            var child = (SquarePuzzle)children[0];
            var space = puzzle.GetSpace();

            Swap(puzzle.Grid, space.Position, child.GetSpace().Position);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userDefined"></param>
    /// <returns></returns>
    public static SquarePuzzle? CreateUserDefined(int[][] userDefined)
    {
        //Error checking, make sure that the provided arrays are qualified to make a square puzzle.
        for (int i = 0; i < userDefined.Length; i++)
            if (userDefined.Length != userDefined[i].Length)
                return null;
        int[][] errorUserDefined = new int[userDefined.Length][];
        for (int i = 0; i < errorUserDefined.Length; i++)
            errorUserDefined[i] = new int[errorUserDefined.Length];
        for (int i = 0; i < errorUserDefined.Length; i++)
            for (int j = 0; j < errorUserDefined.Length; j++)
                errorUserDefined[i][j] = new int();
        int currentNumber = 1;
        for (int i = 0; i < userDefined.Length; i++)
            for (int j = 0; j < userDefined.Length; j++)
            {
                for (int k = 0; k < userDefined.Length; k++)
                    for (int z = 0; z < userDefined.Length; z++)
                        if (userDefined[k][z] == currentNumber)
                        {
                            if (errorUserDefined[i][j] == currentNumber) //check for duplicates.
                                return null;
                            errorUserDefined[i][j] = currentNumber;
                        }
                currentNumber++;
            }
        currentNumber = 1;
        for (int i = 0; i < userDefined.Length; i++)
            for (int j = 0; j < userDefined.Length; j++)
            {
                if (errorUserDefined[i][j] != currentNumber)    // check to make sure we have 1 through length * length
                    return null;
                currentNumber++;
            }
        //End error checking.
        var puzzle = new SquarePuzzle
        {
            Grid = new SquareNode[userDefined.Length][],
        };

        for (int i = 0; i < userDefined.Length; i++)
            puzzle.Grid[i] = new SquareNode[userDefined.Length];

        for (int i = 0; i < userDefined.Length; i++)
            for (int j = 0; j < userDefined.Length; j++)
            {
                puzzle.Grid[i][j] = new SquareNode(new Point(i, j), userDefined[i][j]);
                // if it is the highest number, then it is the "space".
                if (userDefined[i][j] == userDefined.Length * userDefined.Length)
                    puzzle.Grid[i][j].IsSpace = true;
            }
        return puzzle;
    }
    
    /// <summary>
    /// Generates a random square puzzle of size * size.
    /// Not all random puzzles are solvable.
    /// </summary>
    /// <param name="size">Size of the square puzzle. eg size * size.  The size must be >= 2.</param>
    public static SquarePuzzle CreateRandom(int size)
    {
        var start = new SquarePuzzle
        {
            Grid = new SquareNode[size][],
        };
        var rand = new Random();
        for (int i = 0; i < size; i++)
            start.Grid[i] = new SquareNode[size];

        int number = 1;
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                int x;
                int y;
                while (true)
                {
                    x = rand.Next(0, size);
                    y = rand.Next(0, size);
                    if (start.Grid[x][y] != null)
                        continue;
                    break;
                }
                start.Grid[x][y] = new SquareNode(new Point(x, y), number++);
                if (i == size - 1 && j == size - 1)
                    start.Grid[x][y].IsSpace = true;
            }
        return start;
    }
    
    /// <summary>
    /// Creates a copy of the square puzzle that is passed.
    /// </summary>
    public static SquarePuzzle Copy(SquarePuzzle puzzle)
    {
        var copy = new SquarePuzzle
        {
            Grid = new SquareNode[puzzle.Grid.Length][],
        };
        for (int i = 0; i < puzzle.Grid.Length; i++)
            copy.Grid[i] = new SquareNode[puzzle.Grid.Length];

        for (int i = 0; i < puzzle.Grid.Length; i++)
        {
            for (int j = 0; j < puzzle.Grid.Length; j++)
            {
                copy.Grid[i][j] = new SquareNode(puzzle.Grid[i][j].Position, puzzle.Grid[i][j].Number);
                if (puzzle.Grid[i][j].IsSpace)
                {
                    copy.Grid[i][j].IsSpace = true;
                }
            }
        }
        return copy;
    }
}

internal static class ListExtensions
{
    internal static readonly Random Random = new Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}

