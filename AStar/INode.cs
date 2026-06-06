namespace AStar;

/// <summary>
/// The A* algorithm takes a starting node and a goal node and searches from start to the goal.
///
/// The nodes can be setup in a graph ahead of running the algorithm or the children
/// nodes can be generated on the fly when the A* algorithm requests the Children property.
///
/// See the square puzzle implementation to see the children being generated on the fly instead
/// of the classical image/graph search with walls.
/// </summary>
public interface INode
{
    /// <summary>
    /// Gets the total cost for this node.
    /// f = g + h
    /// TotalCost = MovementCost + EstimatedCost.
    /// </summary>
    int TotalCost { get; }

    /// <summary>
    /// Gets the movement cost for this node.
    /// This is the movement cost from this node to the starting node, or g.
    /// </summary>
    int MovementCost { get; }

    /// <summary>
    /// Gets the estimated cost for this node.
    /// This is the heuristic from this node to the goal node, or h.
    /// </summary>
    int EstimatedCost { get; }

    /// <summary>
    /// Gets or sets the parent node to this node.
    /// </summary>
    INode? Parent { get; set; }

    /// <summary>
    /// Gets this node's children.
    /// </summary>
    /// <remarks>The children can be setup in a graph before starting the
    /// A* algorithm or they can be dynamically generated the first time
    /// the A* algorithm calls this property.</remarks>
    IEnumerable<INode> Children { get; }

    /// <summary>
    /// Determines if this node is on the open list.
    /// </summary>
    /// <param name="openList">The AStar open list.</param>
    /// <returns>True if the node is on the open list.</returns>
    bool IsOpenList(IEnumerable<INode> openList);

    /// <summary>
    /// Sets this node to be on the open list.
    /// </summary>
    /// <param name="value">Caches if this node is on the open list or not.</param>
    void SetOpenList(bool value);

    /// <summary>
    /// Determines if this node is on the closed list.
    /// </summary>
    /// <param name="closedList">The AStar closed list.</param>
    /// <returns>True if the node is on the closed list.</returns>
    bool IsClosedList(IEnumerable<INode> closedList);

    /// <summary>
    /// Sets this node to be on the open list.
    /// </summary>
    /// <param name="value">Caches if this node is on the closed list or not.</param>
    void SetClosedList(bool value);

    /// <summary>
    /// Sets the movement cost for the current node, or g.
    /// </summary>
    /// <param name="parent">Parent node, for access to the parents movement cost.</param>
    void SetMovementCost(INode parent);

    /// <summary>
    /// Sets the estimated cost for the current node, or h.
    /// </summary>
    /// <param name="goal">Goal node, for acces to the goals position.</param>
    void SetEstimatedCost(INode goal);

    /// <summary>
    /// Returns true if this node is the goal, false if it is not the goal.
    /// </summary>
    /// <param name="goal">The goal node to compare this node against.</param>
    /// <returns>True if this node is the goal, false if it s not the goal.</returns>
    bool IsGoal(INode goal);
}
