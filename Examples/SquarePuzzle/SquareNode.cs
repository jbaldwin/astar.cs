/* 
astar-1.0.cs may be freely distributed under the MIT license.

Copyright (c) 2013 Josh Baldwin https://github.com/jbaldwin/astar.cs

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without 
restriction, including without limitation the rights to use, 
copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be 
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

namespace AStar.Examples
{
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
}
