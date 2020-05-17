
using System;
using UnityEngine;


namespace pacmac
{
    public class TileGenerator
    {
        public static TileType[,] GenerateTiles(int level, Vector2Int dim)
        {
            TileType[,] grid = new TileType[dim[0], dim[1]];
            /* */
            return grid;
        }

        private static Vector2Int[] GenerateCorners(int level, Vector2Int dim)
        {
            throw new System.NotImplementedException();        
        }
    }

}
