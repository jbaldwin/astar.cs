namespace AStar;

/// <summary>
/// Extension methods to make the System.Collections.Generic.SortedList easier to use.
/// </summary>
internal static class SortedListExtensions
{
    /// <summary>
    /// Checks if the SortedList is empty.
    /// </summary>
    /// <param name="sortedList">SortedList to check if it is empty.</param>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>True if sortedList is empty, false if it still has elements.</returns>
    internal static bool IsEmpty<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
        where TKey : notnull
    {
        return sortedList.Count == 0;
    }

    /// <summary>
    /// Adds a INode to the SortedList.
    /// </summary>
    /// <param name="sortedList">SortedList to add the node to.</param>
    /// <param name="node">Node to add to the sortedList.</param>
    internal static void Add(this SortedList<int, INode> sortedList, INode node)
    {
        sortedList.Add(node.TotalCost, node);
    }

    /// <summary>
    /// Removes the node from the sorted list with the smallest TotalCost and returns that node.
    /// </summary>
    /// <param name="sortedList">SortedList to remove and return the smallest TotalCost node.</param>
    /// <returns>Node with the smallest TotalCost.</returns>
    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    internal static INode Pop(this SortedList<int, INode> sortedList)
    {
        if (sortedList.Count == 0)
        {
            throw new InvalidOperationException("The SortedList is empty.");
        }

        var top = sortedList.Values[0];
        sortedList.RemoveAt(0);
        return top;
    }
}
