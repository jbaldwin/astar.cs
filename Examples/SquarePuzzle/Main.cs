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
	public class Program
	{
		static void Main(string[] args)
		{
			Program p = new Program();
			p.Run();
			Console.In.Read();	//pause so you can see output
		}

		private AStar aStar;
		private SquarePuzzle Current;
		private SquarePuzzle Goal;

		public Program()
		{
			Current = SquarePuzzle.CreateLinear(3);
			SquarePuzzle.Shuffle(Current, 10);	// even a small shuffle can result in huge search times
			Goal = SquarePuzzle.CreateLinear(3);

			Console.WriteLine("Starting position:");
			Current.Print();

			Console.WriteLine("Goal position:");
			Goal.Print();

			aStar = new AStar(Current, Goal);
		}

		public void Run()
		{
			while (true)
			{
				State s = aStar.Step();
				if (s == State.GoalFound || s == State.Failed)
					break;
				if (aStar.Steps % 10000 == 0)
					Console.Out.WriteLine(aStar.Steps + "steps have been performed.");
			}
			var stepsInPath = 0;
			foreach(var node in aStar.GetPath())
			{
				stepsInPath++;
				Console.WriteLine("Step: " + stepsInPath);
				var n = (SquarePuzzle)node;
				n.Print();
			}
			Console.WriteLine("Steps in path: " + stepsInPath);
			Console.Out.WriteLine("Steps to find path: " + aStar.Steps);
		}
	}
}
