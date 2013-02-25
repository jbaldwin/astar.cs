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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar.Examples
{
	public class Grid2D
	{
		public GridNode[][] Grid;

		public int Width { get { return Grid.Length; } }
		public int Height { get { return Grid[0].Length; } }

		public GridNode Start;
		public GridNode Goal;

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
}
