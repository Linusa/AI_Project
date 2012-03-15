namespace AIFGP_Game
{
    using Heuristic = System.Func<Graph<PositionalNode, Edge>, int, int, double>;
    
    public static class AStarHeuristics
    {
        public static Heuristic Distance =
            (g, n1, n2) =>
                (g.GetNode(n1).Position - g.GetNode(n2).Position).Length();
    }
}
