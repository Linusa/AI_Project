﻿namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using AIFGP_Game_Data;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
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
        
        protected List<TileInfo> mapTiles = new List<TileInfo>();

        protected Sprite<byte> grassTile;
        protected Sprite<byte> wallTile;
        protected Sprite<byte> bushTile;

        public NavigationGraph NavigationGraph = new NavigationGraph();
        private Dictionary<Vector2, int> nodeIndices = new Dictionary<Vector2, int>();

        public List<Vector2> getWorldfromNodes(List<int> lookups)
        {
            List<Vector2> temp = new List<Vector2>(lookups.Count);

            for (int i = 0; i < lookups.Count; i++)
                temp.Add(NavigationGraph.GetNode(lookups[i]).Position);

            return temp;
        }

        private GraphSearchViewer navGraphViewer;

        private Timer inputTimer = new Timer(0.2f);
        
        protected List<Vector2> bushes = new List<Vector2>();

        public Map(MapDescription mapDescription)
        {
            WallManager.Instance.Walls.Clear();

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
            
            initializeSprites();

            inputTimer.Start();
        }

        public bool WithinMapBounds(int row, int col)
        {
            return row >= 0 && row < TilesDown && col >= 0 && col < TilesAcross;
        }

        public bool IsTilePosWall(int row, int col)
        {
            return TileInfoAtTilePos(row, col).Type == TileType.Wall;
        }

        public bool IsWorldPosWall(Vector2 worldPosition)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            return IsTilePosWall((int)tilePos.Y, (int)tilePos.X);
        }

        public bool AdjacentToWall(Vector2 worldPosition)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            int row = (int)tilePos.Y;
            int col = (int)tilePos.X;

            int numAdjWalls = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (!WithinMapBounds(i, j) || (i == row && j == col))
                        continue;

                    if (IsTilePosWall(i, j))
                        numAdjWalls++;
                }
            }

            return numAdjWalls > 0;
        }

        public bool IsTilePosBush(int row, int col)
        {
            return TileInfoAtTilePos(row, col).Type == TileType.Bush;
        }
        
        public bool IsWorldPosBush(Vector2 worldPosition)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            return IsTilePosBush((int)tilePos.Y, (int)tilePos.X);
        }

        public TileInfo TileInfoAtTilePos(int row, int col)
        {
            if (col < 0 || col >= TilesAcross)
                throw new System.InvalidOperationException("There are "
                    + TilesAcross + " tiles across and " + col + " was "
                    + " passed in for col.");

            if (row < 0 || row >= TilesDown)
                throw new System.InvalidOperationException("There are "
                    + TilesDown + " tiles down and " + row + " was "
                    + " passed in for row.");

            int tileIdx = (row * TilesAcross) + col;
            return mapTiles[tileIdx];
        }

        public void SetTileInfoAtTilePos(int row, int col, TileInfo tileInfo)
        {
            if (col < 0 || col >= TilesAcross)
                throw new System.InvalidOperationException("There are "
                    + TilesAcross + " tiles across and " + col + " was "
                    + " passed in for col.");

            if (row < 0 || row >= TilesDown)
                throw new System.InvalidOperationException("There are "
                    + TilesDown + " tiles down and " + row + " was "
                    + " passed in for row.");

            int tileIdx = (row * TilesAcross) + col;
            mapTiles[tileIdx] = tileInfo;
        }

        public TileInfo TileInfoAtWorldPos(Vector2 worldPosition)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            return TileInfoAtTilePos((int)tilePos.Y, (int)tilePos.X);
        }

        public void SetTileInfoAtWorldPos(Vector2 worldPosition, TileInfo tileInfo)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            SetTileInfoAtTilePos((int)tilePos.Y, (int)tilePos.X, tileInfo);
        }

        public Vector2 TilePosToWorldPos(Vector2 tilePosition)
        {
            tilePosition.X *= TileSize.X;
            tilePosition.Y *= TileSize.Y;
            return tilePosition + TileSize / 2;
        }

        public Vector2 WorldPosToTilePos(Vector2 worldPos)
        {
            bool xNeg = worldPos.X < 0;
            bool yNeg = worldPos.Y < 0;

            int tileX = (int)(worldPos.X / TileSize.X);
            int tileY = (int)(worldPos.Y / TileSize.Y);

            tileX = xNeg ? tileX - 1 : tileX;
            tileY = yNeg ? tileY - 1 : tileY;

            return new Vector2(tileX, tileY);
        }

        public int ClosestNodeIndex(Vector2 worldPosition)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPosition);
            Vector2 tileCenterWorld = TilePosToWorldPos(tilePos);

            int closestIdx;

            if (!nodeIndices.TryGetValue(tileCenterWorld, out closestIdx))
            {
                int row = (int)tilePos.Y;
                int col = (int)tilePos.X;

                int curIdx = -1;
                float closestDistSquared = float.MaxValue;

                for (int iters = 1; closestDistSquared == float.MaxValue; iters++)
                {
                    for (int i = row - iters; i <= row + iters; i++)
                    {
                        for (int j = col - iters; j <= col + iters; j++)
                        {
                            if ((i > row - iters && i < row + iters && j > col - iters && j < col + iters)
                                || !WithinMapBounds(i, j))
                                continue;

                            Vector2 curTileCenterWorld = TilePosToWorldPos(new Vector2(j, i));
                            if (nodeIndices.TryGetValue(curTileCenterWorld, out curIdx))
                            {
                                Vector2 origTileToCurTile = curTileCenterWorld - tileCenterWorld;
                                float curDistSquared = origTileToCurTile.LengthSquared();
                                if (curDistSquared < closestDistSquared)
                                {
                                    bool occluded = false;

                                    float origTileToCurTileLen = origTileToCurTile.Length();
                                    origTileToCurTile /= origTileToCurTileLen;
                                    Ray ray = new Ray(new Vector3(tileCenterWorld, 0.0f), new Vector3(origTileToCurTile, 0.0f));

                                    foreach (Wall wall in WallManager.Instance.Walls)
                                    {
                                        BoundingBox wallBox = WallManager.Instance.WallExtentsIn3D(wall);
                                        float curIntersectDist = ray.Intersects(wallBox) ?? float.MaxValue;

                                        // Intersection.
                                        if (curIntersectDist < origTileToCurTileLen)
                                        {
                                            occluded = true;
                                            break;
                                        }
                                    }

                                    if (!occluded)
                                    {
                                        closestDistSquared = curDistSquared;
                                        closestIdx = curIdx;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return closestIdx;
        }

        public bool WallsBetween(Vector2 p1, Vector2 p2)
        {
            bool occluded = false;

            Vector2 v = p2 - p1;
            float length = v.Length();

            v /= length;
            Ray ray = new Ray(new Vector3(p1, 0.0f), new Vector3(v, 0.0f));

            if (Single.IsNaN(v.X) || Single.IsNaN(v.Y))
                System.Diagnostics.Debug.WriteLine("V: " + v);

            foreach (Wall wall in WallManager.Instance.Walls)
            {
                BoundingBox wallBox = WallManager.Instance.WallExtentsIn3D(wall);
                float curIntersectDist = ray.Intersects(wallBox) ?? float.MaxValue;

                if (curIntersectDist < length)
                {
                    occluded = true;
                    break;
                }
            }

            return occluded;
        }

        public List<Vector2> BushLocations
        {
            get { return bushes; }
        }
        
        private void createTiles(int[] tileTypeIndices)
        {
            for (int i = 0; i < TilesDown; i++)
            {
                for (int j = 0; j < TilesAcross; j++)
                {
                    TileInfo curTileInfo = new TileInfo();
                    curTileInfo.Position = new Vector2(j * TileSize.X, i * TileSize.Y);

                    int curIdx = (i * TilesAcross) + j;
                    TileType curTileType = (TileType)tileTypeIndices[curIdx];

                    if (curTileType == TileType.Wall)
                    {
                        // Create a Wall and register it with the WallManager.
                        Wall curWall = new Wall();
                        curWall.TopLeftPixel = curTileInfo.Position;
                        curWall.BottomRightPixel = curTileInfo.Position + TileSize;
                        WallManager.Instance.AddWall(curWall);

                        curTileInfo.Type = TileType.Wall;
                    }
                    else if (curTileType == TileType.Bush)
                    {
                        Vector2 bushPos = curTileInfo.Position + TileSize / 2;
                        bushes.Add(bushPos);

                        curTileInfo.Type = TileType.Bush;
                    }
                    else
                        curTileInfo.Type = TileType.Ground;

                    mapTiles.Add(curTileInfo);
                }
            }
        }

        // Map must be loaded before this is called!
        protected virtual void createNavigationGraph()
        {
            // GC may wreak havoc here, test this out.
            nodeIndices.Clear();
            NavigationGraph.Clear();

            computeNavGraphNodes();
            computeNavGraphEdges();
            
            navGraphViewer = new GraphSearchViewer(NavigationGraph);
        }

        private void computeNavGraphNodes()
        {
            Vector2 toTileCenter = TileSize / 2;
            
            foreach (TileInfo mapTile in mapTiles)
            {
                Vector2 tileCenterPos = mapTile.Position + toTileCenter;
                if (!IsWorldPosWall(tileCenterPos) && !AdjacentToWall(tileCenterPos))
                {
                    int nodeIdx = NavigationGraph.AvailableNodeIndex;
                    NavigationGraph.AddNode(new PositionalNode(nodeIdx, tileCenterPos));
                    nodeIndices.Add(tileCenterPos, nodeIdx);
                }
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

        protected virtual void initializeSprites()
        {
            Rectangle tileFrame = new Rectangle(0, 0, (int)TileSize.X, (int)TileSize.Y);
            
            grassTile = new Sprite<byte>(TextureManager.GrassTile, Vector2.Zero, tileFrame);
            grassTile.AddAnimationFrame(0, tileFrame);
            grassTile.ActiveAnimation = 0;
            grassTile.LayerDepth = AStarGame.DrawingOrder.Grass;

            wallTile = new Sprite<byte>(TextureManager.WallTile, Vector2.Zero, tileFrame);
            wallTile.AddAnimationFrame(0, tileFrame);
            wallTile.ActiveAnimation = 0;
            wallTile.LayerDepth = AStarGame.DrawingOrder.Wall;
            
            bushTile = new Sprite<byte>(TextureManager.BushTile, Vector2.Zero, tileFrame);
            bushTile.AddAnimationFrame(0, tileFrame);
            bushTile.ActiveAnimation = 0;
            bushTile.LayerDepth = AStarGame.DrawingOrder.Bush;
        }

        public virtual void Update(GameTime gameTime)
        {
            navGraphViewer.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileInfo tileInfo in mapTiles)
            {
                Sprite<byte> tile;

                if (tileInfo.Type == TileType.Wall)
                    tile = wallTile;
                else if (tileInfo.Type == TileType.Bush)
                {
                    // Hack for now so that the transparent parts
                    // of tall grass do not show bg color.
                    tile = grassTile;
                    tile.Position = tileInfo.Position;
                    tile.Draw(spriteBatch);

                    tile = bushTile;
                }
                else
                    tile = grassTile;

                tile.Position = tileInfo.Position;
                tile.Draw(spriteBatch);
            }

            navGraphViewer.Draw(spriteBatch);
        }
    }
}
