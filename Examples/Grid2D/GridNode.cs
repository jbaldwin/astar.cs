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
using System.Collections.Generic;

namespace AStar.Examples
{
	public class GridNode : INode
	{
		private bool isOpenList = false;
		private bool isClosedList = false;

		public int X { get; private set; }
		public int Y { get; private set; }

		public bool IsWall { get; set; }

		public Grid2D Grid;

		public GridNode(Grid2D grid, int x, int y, bool isWall)
		{
			Grid = grid;
			X = x;
			Y = y;
			IsWall = isWall;
		}

		/// <summary>
		/// Gets or sets whether this node is on the open list.
		/// </summary>
		public bool IsOpenList(IEnumerable<INode> openList)
		{
			return isOpenList;
		}

		public void SetOpenList(bool value)
		{
			isOpenList = value;
		}

		/// <summary>
		/// If this is a wall then return as unsearchable without ever checking the node.
		/// </summary>
		public bool IsClosedList(IEnumerable<INode> closedList)
		{
			return IsWall || isClosedList;
		}

		public void SetClosedList(bool value)
		{
			isClosedList = value;
		}

		/// <summary>
		/// Gets the total cost for this node.
		/// f = g + h
		/// TotalCost = MovementCost + EstimatedCost
		/// </summary>
		public int TotalCost { get { return MovementCost + EstimatedCost; } }

		/// <summary>
		/// Gets the movement cost for this node.
		/// This is the movement cost from this node to the starting node, or g.
		/// </summary>
		public int MovementCost { get; private set; }

		/// <summary>
		/// Gets the estimated cost for this node.
		/// This is the heuristic from this node to the goal node, or h.
		/// </summary>
		public int EstimatedCost { get; private set; }

		/// <summary>
		/// Parent.MovementCost + 1
		/// </summary>
		/// <param name="parent">Parent node, for access to the parents movement cost.</param>
		public void SetMovementCost(INode parent)
		{
			this.MovementCost = parent.MovementCost + 1;
		}

		/// <summary>
		/// Simple manhatten.
		/// </summary>
		/// <param name="goal">Goal node, for acces to the goals position.</param>
		public void SetEstimatedCost(INode goal)
		{
			var g = (GridNode)goal;
			this.EstimatedCost = Math.Abs(this.X - g.X) + Math.Abs(this.Y - g.Y);
		}

		/// <summary>
		/// Gets or sets the parent node to this node.
		/// </summary>
		public INode Parent { get; set; }

		// X - 1, Y - 1 | X, Y - 1 | X + 1, Y - 1
		// X - 1, Y     |          | X + 1, Y
		// X - 1, Y + 1 | X, Y + 1 | X + 1, Y + 1
		//private static int[] childXPos = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
		//private static int[] childYPos = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };

		private static int[] childXPos = new int[] { 0, -1, 1, 0, };
		private static int[] childYPos = new int[] { -1, 0, 0, 1, };

		/// <summary>
		/// Gets this node's children.
		/// </summary>
		/// <remarks>The children can be setup in a graph before starting the
		/// A* algorithm or they can be dynamically generated the first time
		/// the A* algorithm calls this property.</remarks>
		public IEnumerable<INode> Children 
		{
			get
			{
				var children = new List<GridNode>();

				for (int i = 0; i < childXPos.Length; i++)
				{
					// skip any nodes out of bounds.
					if (X + childXPos[i] >= Grid.Width || Y + childYPos[i] >= Grid.Height)
						continue;
					if (X + childXPos[i] < 0 || Y + childYPos[i] < 0)
						continue;

					children.Add(Grid.Grid[X + childXPos[i]][Y + childYPos[i]]);
				}

				return children;
			}
		}

		/// <summary>
		/// Returns true if this node is the goal, false if it is not the goal.
		/// </summary>
		/// <param name="goal">The goal node to compare this node against.</param>
		/// <returns>True if this node is the goal, false if it s not the goal.</returns>
		public bool IsGoal(INode goal)
		{
			return IsEqual((GridNode)goal);
		}

		/// <summary>
		/// Two nodes are equal if they share the same spot in the grid.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsEqual(GridNode node)
		{
			return (this == node) || (this.X == node.X && this.Y == node.Y);
		}

		public string Print(GridNode start, GridNode goal, IEnumerable<INode> path)
		{
			if (IsWall)
			{
				return "W";
			}
			else if (IsEqual(start))
			{
				return "s";
			}
			else if (IsEqual(goal))
			{
				return "g";
			}
			else if (IsInPath(path))
			{
				return ".";
			}
			else
			{
				return " ";
			}
		}

		private bool IsInPath(IEnumerable<INode> path)
		{
			foreach (var node in path)
			{
				if(IsEqual((GridNode)node))
					return true;
			}
			return false;
		}
	}
}
