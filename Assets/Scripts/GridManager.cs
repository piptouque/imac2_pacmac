using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


namespace pacmac
{
    public enum TileType
    {
        EMPTY,
        WALL,
        RIM
    }
    public class GridManager : MonoBehaviour
    {
        private int _level = 0;
        private float _tileSize = 1;

        public Tilemap _wallMap;
        public Tilemap _rimMap;
        public RuleTile _wallTiles;
        public RuleTile _rimTiles;

        // Start is called before the first frame update
        void Start()
        {
            StartLevel(_level);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void StartLevel(int level)
        {
            Vector2Int dim = GenerateDimensions(level);
            TileType[,] grid =  GenerateTiles(level, dim);
            GenerateGrid(grid);
        }

        private void GenerateGrid(TileType[,] grid)
        {
            for (int x=-1; x<=grid.GetLength(0); ++x)
            {
                for (int y=-10; y<=grid.GetLength(1); ++y)
                {
                
                    bool isRim = x<0
                    || x>=grid.GetLength(0)
                    || y<0 || y>=grid.GetLength(1);
                    if (isRim)
                    {
                        _rimMap.SetTile(new Vector3Int(x, y, 0), _rimTiles);
                    }
                    else
                    {
                        switch(grid[x,y])
                        {
                            case TileType.WALL:
                            _wallMap.SetTile(new Vector3Int(x, y, 0), _wallTiles);
                            break;
                            case TileType.EMPTY:
                            default:
                            break;
                        }    
                    }
                }
            }
        }

        private TileType[,] GenerateTiles(int level, Vector2Int dim)
        {
            return TileGenerator.GenerateTiles(level, dim);
        }

        private Vector2Int GenerateDimensions(int level)
        {
            Vector2Int dim = new Vector2Int(5,5);
            return dim;
        }

    }
    
}
