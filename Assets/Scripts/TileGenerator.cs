
using System;
using UnityEngine;

using pacmac.random;


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
        public TileType[,] GenerateTiles(Configuration conf, Vector2Int dim)
        {
            TileType[,] grid = new TileType[dim[0], dim[1]];
            Array.Fill<TileType>(grid, TileType.EMPTY);
            GeneratePaths(grid, conf);
            /* */
            return grid;
        }


        private void GeneratePaths(TileType[,] grid, Configuration conf)
        {
            int numberPaths = conf.RandomPathNumber();
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
                    bool mirrorX = x >= dimX / 4;
                    bool mirrorY = y >= dimY / 4;
                    if (mirrorX || mirrorY)
                    {
                        int indexX, indexY;
                        indexX = mirrorX ? x : dimX - 1 - x;
                        indexX = mirrorY ? y : dimY - 1 - y;
                        grid[x, y] = grid[indexX, indexY];
                    }
                }
            }
        }
        private void LinkCorners(TileType[,] grid, Vector2Int[] corners, int numberPaths, Configuration conf)
        {
            int numberCorners = corners.GetLength(0);
            /*
             * First pass
             * making sure every corner is linked
             */
            {
                for(int i=0; i<numberCorners - 1; ++i)
                {
                    AddPath(grid, corners[i], corners[i+1]);
                }
                /* last two on the border in order to link the four parts */
                int dimX = grid.GetLength(0);
                int dimY = grid.GetLength(1);
                var wayUp = new Vector2Int(conf.RandomX(), dimY / 4);
                var wayRight = new Vector2Int(dimX / 4, RandomY());
                AddPath(grid, wayUp, corners[0]);
                AddPath(grid, corners[i], wayRight);
            }
            for(int i=0; i<numberCorners; ++i)
            {
                Vector2Int first = corners[RandomPathNumber()-1];
                Vector2Int second = corners[RandomPathNumber()-1];
                AddPath(grid, corners[first], corners[second]);
            }
        }

        private static void AddPath(TileType[,] grid, Vector2Int corner1, Vector2Int corner2)
        {
            if(corner1[0] >= corner2[0])
            {
                AddPath(grid, corner2, corner1);
            }
            Vector2Int[] third = new Vector2Int(corner1[0], corner2[1]);
            /* filling path from 1 to 2, going through third. */
            fillPath(grid, corner1, third);
            fillPath(grid, third, corner2);
        }

        private static void fillPath(TileType[,] grid, Vector2Int corner1, Vector2Int corner2)
        {
            if(corner1[0] != corner2[0]
              && corner1[1] != corner2[1])
              {
                  throw new System.ArgumentException("Only use with either vertical or horizontal lines.");
              }
            
            for(int x=Math.Min(corner1[0], corner2[0]); x<=Math.Max(corner1[0], corner2[0]); ++x)
            {
                for(int y=Math.Min(corner1[1], corner2[1]); y<=Math.Max(corner1[1], corner2[1]); ++y)
                {
                    fillCell(grid, x, y);
                }
            }
        }

        private static void fillCell(TileType[,] grid, int x, int y)
        {
            grid[x, y] = TileType.WALL;
        }
        private static int[] GetRangeArray(int length)
        {
            return Enumerable.Range(0, length).ToArray();
        }

    }

}
