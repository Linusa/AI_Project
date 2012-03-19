namespace AIFGP_Game
{
    // Shortcut for an A* heuristic's delegate signature.
    using Heuristic = System.Func<Graph<PositionalNode, Edge>, int, int, double>;

    /// <summary>
    /// AStarHeuristics provides delegates for the various heuristics
    /// AStarSearch could use.
    /// </summary>
    public static class AStarHeuristics
    {
        public static Heuristic Distance =
            (g, n1, n2) =>
                (g.GetNode(n1).Position - g.GetNode(n2).Position).Length();
    }
}
