using System;

namespace SquarePuzzle;

public class NoSpaceException : Exception
{
    /// <summary>
    /// Invalid square puzzle that does not contain a ' ' node.
    /// </summary>
    public SquarePuzzle Puzzle { get; init; }

    /// <summary>
    /// No space exception is thrown when a SquarePuzzle does not contain a ' ' node.
    /// </summary>
    /// <param name="puzzle">The invalid square puzzle.</param>
    public NoSpaceException(SquarePuzzle puzzle)
        : base("No space square was found in this square puzzle state.")
    {
        Puzzle = puzzle;
    }
}
