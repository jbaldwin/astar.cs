using System;
using AStar;

namespace SquarePuzzle;

public class Program
{
    static void Main(string[] args)
    {
        Program p = new Program();
        p.Run();
    }

    private AStar.AStar aStar;
    private SquarePuzzle Current;
    private SquarePuzzle Goal;

    public Program()
    {
        Current = SquarePuzzle.CreateLinear(3);
        SquarePuzzle.Shuffle(Current, 10);    // even a small shuffle can result in huge search times
        Goal = SquarePuzzle.CreateLinear(3);

        Console.WriteLine("Starting position:");
        Current.Print();

        Console.WriteLine("Goal position:");
        Goal.Print();

        aStar = new AStar.AStar(Current, Goal);
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
