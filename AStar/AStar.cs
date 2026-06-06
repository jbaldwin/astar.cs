namespace AStar;

/// <summary>
/// AStar algorithm states while searching for the goal.
/// </summary>
public enum State
{
    /// <summary>
    /// The AStar algorithm is still searching for the goal.
    /// </summary>
    Searching,

    /// <summary>
    /// The AStar algorithm has found the goal.
    /// </summary>
    GoalFound,

    /// <summary>
    /// The AStar algorithm has failed to find a solution.
    /// </summary>
    Failed,
}

/// <summary>
/// System.Collections.Generic.SortedList by default does not allow duplicate items.
/// Since items are keyed by TotalCost there can be duplicate entries per key.
/// </summary>
internal class DuplicateComparer : IComparer<int>
{
    /// <inheritdoc/>
    public int Compare(int x, int y) => (x <= y) ? -1 : 1;
}

/// <summary>
/// Interface to setup and run the AStar algorithm.
/// </summary>
public class AStar
{
    /// <summary>
    /// The open list.
    /// </summary>
    private readonly SortedList<int, INode> openList;

    /// <summary>
    /// The closed list.
    /// </summary>
    private readonly SortedList<int, INode> closedList;

    /// <summary>
    /// The current node.
    /// </summary>
    private INode current;

    /// <summary>
    /// The goal node.
    /// </summary>
    private INode goal;

    /// <summary>
    /// Initializes a new instance of the <see cref="AStar"/> class.
    /// </summary>
    /// <param name="startNode">The starting node for the AStar algorithm.</param>
    /// <param name="goalNode">The goal node for the AStar algorithm.</param>
    public AStar(INode startNode, INode goalNode)
    {
        this.current = startNode;
        this.goal = goalNode;

        var duplicateComparer = new DuplicateComparer();
        this.openList = new SortedList<int, INode>(duplicateComparer);
        this.closedList = new SortedList<int, INode>(duplicateComparer);
        this.Reset(startNode, goalNode);
    }

    /// <summary>
    /// Gets the current amount of steps that the algorithm has performed.
    /// </summary>
    public int Steps { get; private set; }

    /// <summary>
    /// Gets the current state of the open list.
    /// </summary>
    public IEnumerable<INode> OpenList => this.openList.Values;

    /// <summary>
    /// Gets the current state of the closed list.
    /// </summary>
    public IEnumerable<INode> ClosedList => this.closedList.Values;

    /// <summary>
    /// Gets the current node that the AStar algorithm is at.
    /// </summary>
    public INode CurrentNode => this.current;

    /// <summary>
    /// Resets the AStar algorithm with the newly specified start node and goal node.
    /// </summary>
    /// <param name="startNode">The starting node for the AStar algorithm.</param>
    /// <param name="goalNode">The goal node for the AStar algorithm.</param>
    public void Reset(INode startNode, INode goalNode)
    {
        this.openList.Clear();
        this.closedList.Clear();
        this.current = startNode;
        this.goal = goalNode;
        this.openList.Add(this.current);
        this.current.SetOpenList(true);
    }

    /// <summary>
    /// Steps the AStar algorithm forward until it either fails or finds the goal node.
    /// </summary>
    /// <returns>Returns the state the algorithm finished in, Failed or GoalFound.</returns>
    public State Run()
    {
        // Continue searching until either failure or the goal node has been found.
        while (true)
        {
            var s = this.Step();
            if (s != State.Searching)
            {
                return s;
            }
        }
    }

    /// <summary>
    /// Moves the AStar algorithm forward one step.
    /// </summary>
    /// <returns>Returns the state the alorithm is in after the step, either Failed, GoalFound or still Searching.</returns>
    public State Step()
    {
        this.Steps++;
        while (true)
        {
            // There are no more nodes to search, return failure.
            if (this.openList.IsEmpty())
            {
                return State.Failed;
            }

            // Check the next best node in the graph by TotalCost.
            this.current = this.openList.Pop();

            // This node has already been searched, check the next one.
            if (this.current.IsClosedList(this.ClosedList))
            {
                continue;
            }

            // An unsearched node has been found, search it.
            break;
        }

        // Remove from the open list and place on the closed list
        // since this node is now being searched.
        this.current.SetOpenList(false);
        this.closedList.Add(this.current);
        this.current.SetClosedList(true);

        // Found the goal, stop searching.
        if (this.current.IsGoal(this.goal))
        {
            return State.GoalFound;
        }

        // Node was not the goal so add all children nodes to the open list.
        // Each child needs to have its movement cost set and estimated cost.
        foreach (var child in this.current.Children)
        {
            // If the child has already been searched (closed list) or is on
            // the open list to be searched then do not modify its movement cost
            // or estimated cost since they have already been set previously.
            if (child.IsOpenList(this.OpenList) || child.IsClosedList(this.ClosedList))
            {
                continue;
            }

            child.Parent = this.current;
            child.SetMovementCost(this.current);
            child.SetEstimatedCost(this.goal);
            this.openList.Add(child);
            child.SetOpenList(true);
        }

        // This step did not find the goal so return status of still searching.
        return State.Searching;
    }

    /// <summary>
    /// Gets the path of the last solution of the AStar algorithm.
    /// Will return a partial path if the algorithm has not finished yet.
    /// </summary>
    /// <returns>Returns the path from start to goal.</returns>
    public IEnumerable<INode> GetPath()
    {
        var next = this.current;
        var path = new List<INode>();
        while (next != null)
        {
            path.Add(next);
            next = next.Parent;
        }

        path.Reverse();
        return path;
    }
}
