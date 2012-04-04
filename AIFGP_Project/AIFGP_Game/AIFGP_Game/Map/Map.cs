namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using AIFGP_Game_Data;

    using NavigationGraph = Graph<PositionalNode, Edge>;

    /// <summary>
    /// Map represents the game map. It is initialized with a description
    /// representation of the map, and then the navigation graph is
    /// automatically generated via floodfill. A Map can be modified on
    /// the fly and its navigation graph will update accordingly.
    /// </summary>
    public class Map : IDrawable, IUpdateable
    {
        public int TilesAcross;
        public int TilesDown;
        public Vector2 TileSize;
        
        private List<TileInfo> mapTiles = new List<TileInfo>();

        private Sprite<byte> grassTile;
        private Sprite<byte> wallTile;

        public NavigationGraph NavigationGraph = new NavigationGraph();
        private Dictionary<Vector2, int> nodeIndices = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, bool> positionsFilled = new Dictionary<Vector2, bool>();
        private Vector2 seedStart;

        private GraphSearchViewer navGraphViewer;

        private Timer inputTimer = new Timer(0.2f);

        public Map(MapDescription mapDescription)
        {
            TilesAcross = mapDescription.TilesAcross;
            TilesDown = mapDescription.TilesDown;
            TileSize = mapDescription.TileSize;

            if (mapDescription.MapTiles.Length != TilesAcross * TilesDown)
                throw new System.InvalidOperationException("Map area is not "
                    + "equal to tiles across x tiles down! ("
                    + mapDescription.MapTiles.Length + " != "
                    + TilesAcross + " * " + TilesDown + ")");

            createTiles(mapDescription.MapTiles);
            
            createNavigationGraph();
            navGraphViewer = new GraphSearchViewer(NavigationGraph);
            
            initSprites();

            inputTimer.Start();
        }

        public Vector2 TilePosToWorldPos(Vector2 tilePosition)
        {
            tilePosition.X *= TileSize.X;
            tilePosition.Y *= TileSize.Y;
            return tilePosition + TileSize / 2;
        }

        // Map must be loaded before this is called!
        private void createNavigationGraph()
        {
            // GC may wreak havoc here, test this out.
            nodeIndices.Clear();
            positionsFilled.Clear();
            NavigationGraph.Clear();

            Vector2 toTileCenter = TileSize / 2;

            positionsFilled.Clear();
            foreach (TileInfo tileInfo in mapTiles)
                positionsFilled.Add(tileInfo.Position + toTileCenter, false);
            
            seedStart = mapTiles[mapTiles.Count / 2].Position + toTileCenter;

            computeNavGraphNodes(seedStart);
            computeNavGraphEdges();
        }

        // Uses floodfill to auto-generate the navigation graph's nodes.
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
                    int nodeIdx = NavigationGraph.AvailableNodeIndex;
                    NavigationGraph.AddNode(new PositionalNode(nodeIdx, seedPosition));
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

        // After the nodes have been generated, this method iterates
        // over all of them and adds the appropriate edges.
        private void computeNavGraphEdges()
        {
            Vector2 dx = new Vector2(TileSize.X, 0.0f);
            Vector2 dy = new Vector2(0.0f, TileSize.Y);
            double diagDist = Math.Sqrt(dx.X * dx.X + dy.Y * dy.Y);

            foreach (PositionalNode n in NavigationGraph.Nodes)
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
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[leftNodePos], dx.X));

                if (nodeIndices.ContainsKey(rightNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[rightNodePos], dx.X));

                if (nodeIndices.ContainsKey(topNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[topNodePos], dy.Y));

                if (nodeIndices.ContainsKey(bottomNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomNodePos], dy.Y));

                if (nodeIndices.ContainsKey(topLeftNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[topLeftNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(topRightNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[topRightNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(bottomRightNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomRightNodePos], diagDist));
                
                if (nodeIndices.ContainsKey(bottomLeftNodePos))
                    NavigationGraph.AddEdge(new Edge(n.Index, nodeIndices[bottomLeftNodePos], diagDist));
            }
        }

        private void createTiles(int[] tileTypeIndices)
        {
            int xOffset = (AStarGame.WorldDimensions.Width - TilesAcross * (int)TileSize.X) / 2;
            int yOffset = (AStarGame.WorldDimensions.Height - TilesDown * (int)TileSize.Y) / 2;
            Vector2 offset = new Vector2(xOffset, yOffset);

            for (int i = 0; i < TilesDown; i++)
            {
                for (int j = 0; j < TilesAcross; j++)
                {
                    TileInfo curTile = new TileInfo();
                    curTile.Position = offset + new Vector2(j * TileSize.X, i * TileSize.Y);

                    int curIdx = (i * TilesAcross) + j;
                    TileType curTileType = (TileType)tileTypeIndices[curIdx];

                    if (curTileType == TileType.Wall)
                    {
                        // Create a Wall and register it with the WallManager.
                        Wall curWall = new Wall();
                        curWall.TopLeftPixel = curTile.Position;
                        curWall.BottomRightPixel = curTile.Position + TileSize;
                        WallManager.Instance.AddWall(curWall);

                        curTile.Type = TileType.Wall;
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
            
            grassTile = new Sprite<byte>(TextureManager.GrassTile, Vector2.Zero, tileFrame);
            grassTile.AddAnimationFrame(0, tileFrame);
            grassTile.ActiveAnimation = 0;

            wallTile = new Sprite<byte>(TextureManager.WallTile, Vector2.Zero, tileFrame);
            wallTile.AddAnimationFrame(0, tileFrame);
            wallTile.ActiveAnimation = 0;
        }

        public void Update(GameTime gameTime)
        {
            // If left-shift is held down, the user can left-click to put
            // walls down and right-click to remove them. This looks messy,
            // but it's really just handling input.
            if (inputTimer.Expired(gameTime))
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    MouseState mouseState = Mouse.GetState();
                    bool leftMouseButton = mouseState.LeftButton == ButtonState.Pressed;
                    bool rightMouseButton = mouseState.RightButton == ButtonState.Pressed;
                    if (leftMouseButton || rightMouseButton)
                    {
                        int leftRightPad = (AStarGame.WorldDimensions.Width - (TilesAcross * (int)TileSize.X)) / 2;
                        int topBottomPad = (AStarGame.WorldDimensions.Height - (TilesDown * (int)TileSize.Y)) / 2;

                        Vector2 mouseVec = AStarGame.MousePositionInWorld();

                        int mapSquareX = ((int)mouseVec.X - leftRightPad) / (int)TileSize.X;
                        int mapSquareY = ((int)mouseVec.Y - topBottomPad) / (int)TileSize.Y;

                        mapSquareX = (int)MathHelper.Clamp(mapSquareX, 0, TilesAcross - 1);
                        mapSquareY = (int)MathHelper.Clamp(mapSquareY, 0, TilesDown - 1);

                        int mapSquareIndex = (mapSquareY * TilesAcross) + mapSquareX;

                        TileInfo selectedTile = mapTiles[mapSquareIndex];

                        Wall wall = new Wall();
                        wall.TopLeftPixel = selectedTile.Position;
                        wall.BottomRightPixel = selectedTile.Position + TileSize;

                        // Update the GameMap, walls, and navigation graph if a wall was
                        // placed/removed.
                        if (leftMouseButton)
                        {
                            Vector2 nodePos = selectedTile.Position + TileSize / 2;
                            if (nodeIndices.ContainsKey(nodePos))
                            {
                                int selectedNodeIdx = nodeIndices[nodePos];
                                PositionalNode selectedNode = NavigationGraph.GetNode(selectedNodeIdx);

                                selectedTile.Type = TileType.Wall;
                                WallManager.Instance.AddWall(wall);

                                bool graphWasDisplayed = navGraphViewer.DisplayGraph;
                                bool indicesWereDisplayed = navGraphViewer.DisplayNodeIndices;

                                createNavigationGraph();
                                navGraphViewer = new GraphSearchViewer(NavigationGraph);

                                navGraphViewer.DisplayGraph = graphWasDisplayed;
                                navGraphViewer.DisplayNodeIndices = indicesWereDisplayed;
                            }
                        }
                        else if (rightMouseButton)
                        {
                            selectedTile.Type = TileType.Ground;
                            WallManager.Instance.RemoveWall(wall);
                            
                            bool graphWasDisplayed = navGraphViewer.DisplayGraph;
                            bool indicesWereDisplayed = navGraphViewer.DisplayNodeIndices;

                            createNavigationGraph();
                            navGraphViewer = new GraphSearchViewer(NavigationGraph);

                            navGraphViewer.DisplayGraph = graphWasDisplayed;
                            navGraphViewer.DisplayNodeIndices = indicesWereDisplayed;
                        }

                        mapTiles[mapSquareIndex] = selectedTile;
                    }
                }
            }

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
