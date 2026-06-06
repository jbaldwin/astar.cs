using System;

namespace SquarePuzzle;

/// <summary>
/// The square puzzle is made up of x * x nodes.
/// Each node within the square puzzle has a specific number, 
/// and one of the nodes in the square puzzle is a "space" node.
/// The space node is the node that moves around from each state to
/// generate the children square puzzles.
/// </summary>
public class SquareNode
{
    /// <summary>
    /// The number of this node in the SquarePuzzle.
    /// </summary>
    public int Number;
    
    /// <summary>
    /// Gets or sets if this node is the "space" node, if this is true then
    /// this node does not have a corresponding number.
    /// </summary>
    public bool IsSpace { get; set; }
    
    /// <summary>
    /// Gets or sets the position of this node in the square puzzle.
    /// </summary>
    public Point Position { get; set; }
    
    /// <summary>
    /// Creates a new node for the square puzzle.
    /// </summary>
    /// <param name="position">The current position of this node in the square puzzle.</param>
    /// <param name="number">The number of this node.</param>
    public SquareNode(Point position, int number)
    {
        Position = position;
        Number = number;
    }
    
    /// <summary>
    /// Returns the node's number.  If it is the "space" square then " ".
    /// </summary>
    public new String ToString()
    {
        return IsSpace ? " " : Number.ToString();
    }
}

/// <summary>
/// X and Y positioning.
/// </summary>
public class Point
{
    /// <summary>
    /// The X position.
    /// </summary>
    public int X;
    /// <summary>
    /// The Y position.
    /// </summary>
    public int Y;
    /// <summary>
    /// Creates a new point.
    /// </summary>
    /// <param name="x">The X position.</param>
    /// <param name="y">The Y position.</param>
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
