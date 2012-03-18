namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.IO;

    using NavigationGraph = Graph<PositionalNode, Edge>;

    public class Map : IDrawable, IUpdateable
    {
        public enum TileType
        {
            Ground,
            Wall
        }

        public int TilesAcross;
        public int TilesDown;

        // Map tiles are 28x28 in px.
        public static readonly Vector2 TileSize
            = new Vector2(28.0f, 28.0f);
        
        private List<TileInfo> mapTiles = new List<TileInfo>();

        private Sprite<byte> grassTile;
        private Sprite<byte> wallTile;

        private NavigationGraph navGraph = new NavigationGraph();
        private Dictionary<Vector2, int> nodeIndices = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, bool> positionsFilled = new Dictionary<Vector2, bool>();
        private Vector2 seedStart;

        private GraphViewer navGraphViewer;

        private string mapDirectory;

        public Map(string mapFileName)
        {
            mapDirectory = @"C:\Users\Jason\Documents\AI_Project\AIFGP_Project\AIFGP_Game\AIFGP_Game\Map\ascii_maps\";
            loadMapFromText(mapFileName);

            CreateNavigationGraph();
            navGraphViewer = new GraphViewer(navGraph);

            /*
            AStarSearch search = new AStarSearch(navGraph, 451, 112, AStarHeuristics.Distance);
            if (search.TargetFound)
            {
                List<int> path;
                search.PathToTarget(out path);

                for (int i = 0; i < path.Count - 1; i++)
                    navGraphViewer.ChangeEdgeColor(navGraph.GetEdge(path[i], path[i+1]), Color.Red);
            }
            */

            initSprites();
        }

        // Map must be loaded before this is called!
        public void CreateNavigationGraph()
        {
            Vector2 toTileCenter = TileSize / 2;

            positionsFilled.Clear();
            foreach (TileInfo tileInfo in mapTiles)
                positionsFilled.Add(tileInfo.Position + toTileCenter, false);

            computeNavGraphNodes(seedStart);
            computeNavGraphEdges();

            // GC may wreak havoc here, test this out.
            nodeIndices.Clear();
            positionsFilled.Clear();
        }

        private void computeNavGraphNodes(Vector2 seedPosition)
        {
            if (positionsFilled.ContainsKey(seedPosition) && !positionsFilled[seedPosition])
            {
                bool tileIsWall = false;
                foreach (Wall wall in WallManager.Instance.Walls)
                    if (wall.BoundingBox.Contains((int)seedPosition.X, (int)seedPosition.Y))
                        tileIsWall = true;

                if (!tileIsWall)
                {
                    int nodeIdx = navGraph.AvailableNodeIndex;
                    navGraph.AddNode(new PositionalNode(nodeIdx, seedPosition));
                    nodeIndices.Add(seedPosition, nodeIdx);
                }

                positionsFilled[seedPosition] = true;
                
                Vector2 dx = new Vector2(TileSize.X, 0.0f);
                Vector2 dy = new Vector2(0.0f, TileSize.Y);

                Vector2 leftCellPos = seedPosition - dx;
                Vector2 rightCellPos = seedPosition + dx;
                Vector2 topCellPos = seedPosition - dy;
                Vector2 bottomCellPos = seedPosition + dy;

                computeNavGraphNodes(leftCellPos);
                computeNavGraphNodes(rightCellPos);
                computeNavGraphNodes(topCellPos);
                computeNavGraphNodes(bottomCellPos);
            }
        }

        private void computeNavGraphEdges()
        {
            Vector2 dx = new Vector2(TileSize.X, 0.0f);
            Vector2 dy = new Vector2(0.0f, TileSize.Y);
            double diagDist = Math.Sqrt(dx.X * dx.X + dy.Y * dy.Y);

            foreach (PositionalNode n in navGraph.Nodes)
            {
                Vector2 leftNodePos = n.Position - dx;
                Vector2 topLeftNodePos = n.Position - dx - dy;
                Vector2 topNodePos = n.Position - dy;
                Vector2 topRightNodePos = n.Position + dx - dy;
                Vector2 rightNodePos = n.Position + dx;
                Vector2 bottomRightNodePos = n.Position + dx + dy;
                Vector2 bottomNodePos = n.Position + dy;
                Vector2 bottomLeftNodePos = n.Position - dx + dy;

                if (nodeIndices.ContainsKey(leftNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[leftNodePos], dx.X));

                if (nodeIndices.ContainsKey(rightNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[rightNodePos], dx.X));

                if (nodeIndices.ContainsKey(topNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[topNodePos], dy.Y));

                if (nodeIndices.ContainsKey(bottomNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomNodePos], dy.Y));

                if (nodeIndices.ContainsKey(topLeftNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[topLeftNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(topRightNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[topRightNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(bottomRightNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomRightNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(bottomLeftNodePos))
                    navGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomLeftNodePos], diagDist));
            }
        }

        private void loadMapFromText(string mapFileName)
        {
            string path = mapDirectory + mapFileName;

            if (!File.Exists(mapDirectory + mapFileName))
            {
                System.Diagnostics.Debug.WriteLine("Map '" + mapFileName
                    + "' does not exist!");
                return;
            }

            string[] textMap = File.ReadAllLines(path);

            TilesAcross = textMap[0].Length;
            TilesDown = textMap.Length;

            int xOffset = (AStarGame.ScreenDimensions.Width - TilesAcross * (int)TileSize.X) / 2;
            int yOffset = (AStarGame.ScreenDimensions.Height - TilesDown * (int)TileSize.Y) / 2;
            Vector2 offset = new Vector2(xOffset, yOffset);

            for (int i = 0; i < textMap.Length; i++)
            {
                char[] chars = textMap[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    TileInfo curTile = new TileInfo();
                    curTile.Position = offset + new Vector2(j * TileSize.X, i * TileSize.Y);

                    if (chars[j] == 'W')
                    {
                        // Create a Wall and register it with the WallManager.
                        Wall curWall = new Wall();
                        curWall.TopLeftPixel = curTile.Position;
                        curWall.BottomRightPixel = curTile.Position + TileSize;
                        WallManager.Instance.AddWall(curWall);

                        curTile.Type = TileType.Wall;
                    }
                    else if (chars[j] == '+')
                    {
                        seedStart = curTile.Position + (TileSize / 2);
                        curTile.Type = TileType.Ground;
                    }
                    else
                        curTile.Type = TileType.Ground;

                    mapTiles.Add(curTile);
                }
            }
        }

        private void initSprites()
        {
            Rectangle tileFrame = new Rectangle(0, 0, (int)TileSize.X, (int)TileSize.Y);
            
            grassTile = new Sprite<byte>(AStarGame.GrassTile, Vector2.Zero, tileFrame);
            grassTile.AddAnimationFrame(0, tileFrame);
            grassTile.ActiveAnimation = 0;

            wallTile = new Sprite<byte>(AStarGame.WallTile, Vector2.Zero, tileFrame);
            wallTile.AddAnimationFrame(0, tileFrame);
            wallTile.ActiveAnimation = 0;
        }

        public void Update(GameTime gameTime)
        {
            navGraphViewer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileInfo tileInfo in mapTiles)
            {
                Sprite<byte> tile;

                if (tileInfo.Type == TileType.Wall)
                    tile = wallTile;
                else
                    tile = grassTile;

                tile.Position = tileInfo.Position;
                tile.Draw(spriteBatch);
            }

            navGraphViewer.Draw(spriteBatch);
        }
    }
}
