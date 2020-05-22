
using System;
using System.Linq;
using UnityEngine;

namespace pacmac
{
    public enum TileType
    {
        EMPTY,
        WALL
    }
    public class TileGenerator
    {
        public TileGenerator()
        {
        }

        public TileType[,] GenerateTiles(Configuration conf)
        {
            Vector2Int dim = conf.GetLevelDimensions();
            TileType[,] grid = new TileType[dim[0], dim[1]];
            for (int x=0; x<grid.GetLength(0); ++x)
            {
                for (int y=0; y<grid.GetLength(1); ++y)
                {
                    grid[x, y] = TileType.WALL;
                }
            }
            GeneratePaths(grid, conf);
            /* */
            return grid;
        }

        private void GeneratePaths(TileType[,] grid, Configuration conf)
        {
            int numberPaths = conf.GetNumberPaths();
            var corners = new Vector2Int[numberPaths];
            for(int i=0; i<numberPaths; ++i)
            {
                corners[i] = conf.RandomCoords();
            }
            LinkCorners(grid, corners, numberPaths, conf);
            MirrorPaths(grid);
        }

        private void MirrorPaths(TileType[,] grid)
        {
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            for (int x=0; x<dimX; ++x)
            {
                for (int y=0; y<dimY; ++y)
                {
                    bool mirrorX = x >= dimX / 2;
                    bool mirrorY = y >= dimY / 2;
                    if (mirrorX || mirrorY)
                    {
                        int indexX, indexY;
                        indexX = mirrorX ? dimX - 1 - x : x;
                        indexY = mirrorY ? dimY - 1 - y : y;
                        grid[x, y] = grid[indexX, indexY];
                    }
                }
            }
        }

        private void LinkCorners(TileType[,] grid, Vector2Int[] corners, int numberPaths, Configuration conf)
        {
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            int numberCorners = corners.GetLength(0);
            /*
             * First pass
             * making sure every corner is linked
             */
            {
                for (int i=0; i<numberCorners - 1; ++i)
                {
                    AddPath(grid, corners[i], corners[i + 1]);
                }
                /* last two on the border in order to link the four parts */
                var wayUp = new Vector2Int(conf.RandomX(), dimY / 4);
                var wayRight = new Vector2Int(dimX / 4, conf.RandomY());
                AddPath(grid, wayRight, wayUp);
                if (numberCorners > 1)
                {
                    AddPath(grid, wayUp, corners[0]);
                    AddPath(grid, wayRight, corners[numberCorners - 1]);
                }
            }
            for(int i=0; i<numberCorners; ++i)
            {
                Vector2Int first = corners[conf.RandomPathIndex()];
                Vector2Int second = corners[conf.RandomPathIndex()];
                AddPath(grid, first, second);
            }
        }

        private static void AddPath(TileType[,] grid, Vector2Int corner1, Vector2Int corner2)
        {
            if(corner1[0] > corner2[0])
            {
                AddPath(grid, corner2, corner1);
            }
            Vector2Int third = new Vector2Int(corner1[0], corner2[1]);
            /* filling path from 1 to 2, going through third. */
            freePath(grid, corner1, third);
            freePath(grid, third, corner2);
        }

        private static void freePath(TileType[,] grid, Vector2Int corner1, Vector2Int corner2)
        {
            if(corner1[0] != corner2[0]
              && corner1[1] != corner2[1])
            {
                  throw new System.ArgumentException("Only use with either vertical or horizontal lines.");
            }
            int minX = Math.Min(corner1[0], corner2[0]);
            int maxX = Math.Max(corner1[0], corner2[0]);
            int minY = Math.Min(corner1[1], corner2[1]);
            int maxY = Math.Max(corner1[1], corner2[1]);
            for(int x=minX; x<=maxX; ++x)
            {
                for(int y=minY; y<=maxY; ++y)
                {
                    freeCell(grid, x, y);
                }
            }
        }

        private static void freeCell(TileType[,] grid, int x, int y)
        {
            grid[x, y] = TileType.EMPTY;
        }
    }

}
